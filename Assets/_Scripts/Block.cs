using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	public static Color GetRandomColor() {
		System.Random random = new System.Random ();
		int value = random.Next (7);

		switch (value) {
		case 0:
			return Color.yellow;
		case 1:
			return new Color (0.13f, 0.59f, 0.95f);
		case 2:
			return Color.green;
		case 3:
			return Color.red;
		case 4:
			return Color.magenta;
		case 5:
			return Color.cyan;
		case 6: // Orange
			return new Color (1.0f, 0.596f, 0.0f);
		default:
			return Color.black;
		}
	}
}