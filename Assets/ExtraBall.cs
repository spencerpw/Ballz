using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D col) {
		Messenger.Broadcast("ExtraBall");
		Destroy(gameObject);
	}
}
