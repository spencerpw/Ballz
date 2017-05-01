﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Row : MonoBehaviour {
	public const float SLIDE_DURATION = 0.5f;

	public enum SlotType{
		BLANK,
		TILE,
		BALL,
		POINT
	}

	public HorizontalLayoutGroup layout;
	public List<GameObject> occupants;

	private void Start() {
		LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
		layout.enabled = false;
	}
}
