using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colours : Singleton<Colours> {
	protected Colours () {}

	[SerializeField] private Color[] ranges;

	public Color[] Ranges {
		get {
			return ranges;
		}
	}
}
