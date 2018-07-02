using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
	public GameObject mainMenu;
	public GameObject gameOverMenu;
	public GameObject pauseMenu;
	public Menu ActiveMenu {
		get { return activeMenu; }
	}

	private Menu activeMenu;
	private bool uiIsDisplayed;

	void Start () {
		activeMenu = mainMenu.GetComponent<Menu> ();
		uiIsDisplayed = true;
	}

	public void RevealUI() {
		GameObject.Find ("Mask").GetComponent<Animator> ().SetTrigger ("Show Menu");
		GameObject controls = GameObject.FindGameObjectWithTag ("GameController");
		controls.GetComponent<InputManager> ().ControlState = controls.GetComponent<BlockedControlState> ();
		uiIsDisplayed = true;
	}

	// Called by the animator when the fade-in animation completes
	public void EndRevealUI() {
		GameObject controls = GameObject.FindGameObjectWithTag ("GameController");
		controls.GetComponent<InputManager> ().ControlState = controls.GetComponent<MenuControlState> ();
	}

	public void HideUI() {
		GameObject.Find ("Mask").GetComponent<Animator> ().SetTrigger ("Hide Menu");
		GameObject controls = GameObject.FindGameObjectWithTag ("GameController");
		controls.GetComponent<InputManager> ().ControlState = controls.GetComponent<BlockedControlState> ();
		uiIsDisplayed = false;
	}

	// Called by the animator when the fade-out animation completes
	public void EndHideUI() {
		mainMenu.SetActive (false);
		gameOverMenu.SetActive (false);
		pauseMenu.SetActive (false);
	}

	public void ShowMainMenu() {
		ShowMenu (mainMenu);
	}

	public void ShowGameOverMenu() {
		ShowMenu (gameOverMenu);
	}

	public void ShowPauseMenu() {
		ShowMenu (pauseMenu);
	}

	void ShowMenu(GameObject targetMenu) {
		targetMenu.SetActive (true);
		activeMenu = targetMenu.GetComponent<Menu> ();
		if (!uiIsDisplayed)
			RevealUI ();
	}
}