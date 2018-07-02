using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : Menu {
	public GameObject mainMenuButton;
	public GameObject exitButton;

	void Start () {
		SetUp ();
		allButtons.Add (mainMenuButton);
		allButtons.Add (exitButton);
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Bold;
	}

	override public void Press() {
		if (ActiveButton == mainMenuButton) {
			GameObject.FindGameObjectWithTag ("UI").GetComponent<UIManager> ().ShowMainMenu ();
			gameObject.SetActive (false);
		} else if (ActiveButton == exitButton) {
			Application.Quit ();
		}
	}
}