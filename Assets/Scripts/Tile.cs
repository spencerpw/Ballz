using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
	public TextMeshProUGUI valueLabel;
	public int value;
	public Image bg;

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

		SetColour();
	}

	private void OnCollisionEnter2D(Collision2D col) {
		value--;
		RefreshLabel();
	}

	private void SetColour() {
		float t = (float)value%10f / 10f;
		int i = value/10 % 7;
		int j = (i+1)%7;
		Color c = Color.Lerp(Colours.Instance.Ranges[i],Colours.Instance.Ranges[j],t);
		bg.color = c;
	}
}
