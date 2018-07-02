using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControlState : MonoBehaviour, ControlObserver {
	private UIManager uiManager;

	void Start() {
		GameObject ui = GameObject.FindGameObjectWithTag ("UI");
		uiManager = ui.GetComponent<UIManager> ();
	}

	public void Notify(Dictionary<KeyCode, KeyState> inputs) {
		HandleMenuControls(inputs);
	}

	private void HandleMenuControls(Dictionary<KeyCode, KeyState> inputs) {
		if (inputs [KeyCode.DownArrow] == KeyState.Pressed)
			uiManager.ActiveMenu.SelectNext ();
		else if (inputs [KeyCode.UpArrow] == KeyState.Pressed)
			uiManager.ActiveMenu.SelectPrevious ();
		else if (inputs [KeyCode.Return] == KeyState.Pressed)
			uiManager.ActiveMenu.Press ();
	}
}
