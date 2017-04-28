using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D col) {
		Messenger.Broadcast("Point");
		Destroy(gameObject);
	}
}
