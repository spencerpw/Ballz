using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Shooter : MonoBehaviour {
	public int balls = 1;
	public float speed = 1;
	public GameObject aim;
	public GameObject aimBalls;
	public TextMeshProUGUI countLabel;
	public float maxMagnitude = 15f;
	public float scaleThreshold = 0.5f;

	private bool canShoot;
	private Vector2 initialTouchPos;
	private Vector2 direction;

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
			float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg + 90f;
			aim.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			aim.transform.localScale = Vector3.one * scale;
			aim.SetActive(e.position.y < initialTouchPos.y);
		}
	}

	public void TryShoot(PointerEventData e) {
		if(canShoot && aim.activeInHierarchy) {
		}

		aim.SetActive(false);
	}
}
