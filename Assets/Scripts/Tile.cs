using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour {
	public TextMeshProUGUI valueLabel;
	public int value;

	private void Start() {
		if(Random.value > 0.5f)
			value *= 2;
		RefreshLabel();
	}

	private void RefreshLabel() {
		if(value > 0)
			valueLabel.text = value.ToString();
		else
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D col) {
		value--;
		RefreshLabel();
	}
}
