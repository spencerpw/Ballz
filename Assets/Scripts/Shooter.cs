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
	private GameObject landedBall;

	private void Awake() {
		Messenger.AddListener<Ball>("HitBottom",BallHitBottom);
		Messenger.AddListener("ExtraBall",ExtraBall);
	}

	private void Start() {
		Ready();
	}

	public void Ready() {
		canShoot = true;
		gameObject.SetActive(true);
		countLabel.gameObject.SetActive(true);
		countLabel.text = string.Format("x{0}",balls);
		if(landedBall != null) {
			transform.position = landedBall.transform.position;
			Destroy(landedBall.gameObject);
		}
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
			StartCoroutine(ShootRoutine(balls));
		}

		aim.SetActive(false);
	}

	private IEnumerator ShootRoutine(int b) {
		for(int i  = 0; i < b; i++) {
			Shoot();
			countLabel.text = string.Format("x{0}",b-i);
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
		if(landedBall == null) {
			landedBall = b.gameObject;
			landedBall.transform.SetParent(transform.parent);
			landedBall.transform.localPosition = new Vector3(landedBall.transform.localPosition.x, 0, 0);
		} else {
			b.transform.DOMove(landedBall.transform.position,0.1f)
				.OnComplete( () => {
					Destroy(b.gameObject);
				});
		}

		shotBalls--;

		if(shotBalls == 0) {
			Messenger.Broadcast("SpawnRow");
			Ready();
		}
		
	}

	private void ExtraBall() {
		balls++;
	}
}
