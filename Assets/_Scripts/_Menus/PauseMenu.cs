using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Menu {
	public GameObject resumeButton;
	public GameObject mainMenuButton;

	void Start () {
		SetUp ();
		allButtons.Add (resumeButton);
		allButtons.Add (mainMenuButton);
		ActiveButton.GetComponent<Text> ().fontStyle = FontStyle.Bold;
	}

	override public void Press () {
		if (ActiveButton == resumeButton) {
			uiManager.HideUI ();
			GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().Resume ();
		}
	}
}