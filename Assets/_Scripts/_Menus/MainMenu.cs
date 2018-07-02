using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu {
	public GameObject newGame;
	public GameObject exitButton;

	void Start () {
		SetUp ();
		allButtons.Add (newGame);
		allButtons.Add (exitButton);
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Bold;
	}

	override public void Press () {
		if (ActiveButton == newGame) {
			uiManager.HideUI ();
			GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().StartNewGame ();
		} else if (ActiveButton == exitButton) {
			Application.Quit ();
		}
	}
}