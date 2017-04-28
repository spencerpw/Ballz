using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Shooter : MonoBehaviour {
	public int balls = 1;
	public float speed = 1;
	public GameObject aim;
	public Transform ballRoot;
	public Ball ballPrefab;
	public TextMeshProUGUI countLabel;
	public float maxMagnitude = 15f;
	public float scaleThreshold = 0.5f;
	public float ballSpeed = 1f;
	public float shotDelay = 0.2f;


	private bool canShoot;
	private Vector2 initialTouchPos;
	private Vector2 direction;
	private int shotBalls;

	private void Awake() {
		Messenger.AddListener<Ball>("HitBottom",BallHitBottom);
	}

	private void Start() {
		Ready();
	}

	public void Ready() {
		canShoot = true;
		countLabel.gameObject.SetActive(true);
		countLabel.text = string.Format("x{0}",balls);
	}

	public void TryBeginAim(PointerEventData e) {
		if(canShoot) {
			initialTouchPos = e.position;
		}
	}

	public void TryAim(PointerEventData e) {
		if(canShoot) {
			direction = initialTouchPos - e.position;
			float mag = (initialTouchPos - e.position).magnitude;
			float scale = Mathf.Clamp(mag/maxMagnitude, scaleThreshold, 1f);
			float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg + 90f; //Weird, but it works. Would come up with something better in more time
			aim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			aim.transform.localScale = Vector3.one * scale;
			aim.SetActive(e.position.y < initialTouchPos.y);
		}
	}

	public void TryShoot(PointerEventData e) {
		if(canShoot && aim.activeInHierarchy) {
			canShoot = false;
			shotBalls = balls;
			StartCoroutine(ShootRoutine());
		}

		aim.SetActive(false);
	}

	private IEnumerator ShootRoutine() {
		for(int i  = 0; i < balls; i++) {
			Shoot();
			countLabel.text = string.Format("x{0}",balls-i);
			yield return new WaitForSeconds(shotDelay);
		}

		this.gameObject.SetActive(false);
		countLabel.gameObject.SetActive(false);
	}

	private void Shoot() {
		Ball b = Instantiate<Ball>(ballPrefab);
		b.transform.SetParent(ballRoot,false);
		b.transform.position = this.transform.position;
		b.transform.localScale = Vector3.one;

		b.rigidbody2d.velocity = direction.normalized * ballSpeed;
	}

	private void BallHitBottom(Ball b) {
		if(!gameObject.activeInHierarchy) {
			gameObject.SetActive(true);
			transform.position = b.transform.position;
			transform.localPosition = new Vector3(transform.localPosition.x, 0, 0);
			Destroy(b.gameObject);
		} else {
			b.transform.DOMove(transform.position,0.1f)
				.OnComplete( () => {
					Destroy(b.gameObject);
				});
		}

		shotBalls--;

		if(shotBalls == 0) {
			Ready();
		}
		
	}
}
