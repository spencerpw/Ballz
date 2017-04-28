using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public Rigidbody2D rigidbody2d;


	private void OnCollisionEnter2D(Collision2D col) {
		if(col.collider.CompareTag("Bottom")) {
			rigidbody2d.velocity = Vector2.zero;
			Messenger.Broadcast<Ball>("HitBottom",this);
		}
	}
}
