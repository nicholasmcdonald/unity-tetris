using System.Collections.Generic;
using UnityEngine;

public class GameplayControlState : MonoBehaviour, ControlObserver {
	private const float LONG_PRESS_LATERAL = 0.2f;
	private const float LONG_PRESS_VERTICAL = 0.15f;

	private TetrominoManager tetrominoManager;
	private GameManager gameManager;
	private KeyCode activeArrowKey;
	private Timer timer;
	private bool arrowKeyHeld = false;

	void Start() {
		GameObject gm = GameObject.FindGameObjectWithTag ("GameManager");
		gameManager = gm.GetComponent<GameManager> ();
		tetrominoManager = gm.GetComponent<TetrominoManager> ();
		timer = GameObject.FindGameObjectWithTag ("GameClock").GetComponent<GameClock> ().GetNewTimer ();
		timer.SetTimeout (LONG_PRESS_LATERAL);
		activeArrowKey = KeyCode.None;
	}

	public void Notify(Dictionary<KeyCode, KeyState> inputs) {
		HandleEscKey (inputs);
		HandleArrowKeys (inputs);
		HandleDownArrow (inputs);
		HandleRotationKeys (inputs);
	}

	private void HandleEscKey(Dictionary<KeyCode, KeyState> inputs) {
		if (inputs [KeyCode.Escape] == KeyState.Pressed) {
			timer.Stop ();
			// Deactivate GameplayControlState
			// Open pause menu
			// Pause game
		}
	}

	private void HandleDownArrow(Dictionary<KeyCode, KeyState> inputs) {
		KeyState state = inputs [KeyCode.DownArrow];

		if (state == KeyState.Released && activeArrowKey == KeyCode.DownArrow) {
			tetrominoManager.UnblockFastDrop ();
			activeArrowKey = KeyCode.None;
		} else if ((state == KeyState.Pressed || state == KeyState.Held) && activeArrowKey == KeyCode.None) {
			ActivateArrowKey (KeyCode.DownArrow);
		}
	}

	private void HandleRotationKeys(Dictionary<KeyCode, KeyState> inputs) {
		if (inputs [KeyCode.A] == KeyState.Pressed)
			tetrominoManager.Rotate (KeyCode.A);
		else if (inputs [KeyCode.D] == KeyState.Pressed)
			tetrominoManager.Rotate (KeyCode.D);
	}

	private void HandleArrowKeys(Dictionary<KeyCode, KeyState> inputs) {
		if (inputs [KeyCode.LeftArrow] != KeyState.None) {
			ActivateArrowKey (KeyCode.LeftArrow);
		} else if (inputs [KeyCode.RightArrow] != KeyState.None) {
			ActivateArrowKey (KeyCode.RightArrow);
		} else if (inputs [KeyCode.DownArrow] != KeyState.None) {
			ActivateArrowKey (KeyCode.DownArrow);
		} else {
			ClearActiveArrowKey ();
		}

		ExecuteArrowAction ();
	}

	private void ActivateArrowKey(KeyCode key) {
		if (activeArrowKey != key) {
			activeArrowKey = key;
			timer.StartCountDown ();
			arrowKeyHeld = false;
		} else {
			arrowKeyHeld = true;
		}
	}

	private void ClearActiveArrowKey() {
		activeArrowKey = KeyCode.None;
		timer.Stop ();
	}

	private void ExecuteArrowAction() {
		if (activeArrowKey == KeyCode.LeftArrow || activeArrowKey == KeyCode.RightArrow)
			ExecuteLateralKeyAction ();
		else if (activeArrowKey == KeyCode.DownArrow)
			ExecuteDownKeyAction ();
	}

	private void ExecuteDownKeyAction() {
		if (!timer.Ready () && !arrowKeyHeld) {
			timer.SetTimeout (LONG_PRESS_VERTICAL);
			timer.StartCountDown ();
			tetrominoManager.FastDrop ();
		} else if (timer.Ready () && arrowKeyHeld) {
			timer.Lap ();
			tetrominoManager.FastDrop ();
		}
	}

	private void ExecuteLateralKeyAction() {
		if (!timer.Ready () && !arrowKeyHeld) {
			timer.SetTimeout (LONG_PRESS_LATERAL);
			timer.StartCountDown ();
			tetrominoManager.MoveLaterally (activeArrowKey);
		} else if (timer.Ready () && arrowKeyHeld) {
			timer.Lap ();
			tetrominoManager.MoveLaterally (activeArrowKey);
		}
	}
}