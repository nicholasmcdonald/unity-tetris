using System.Collections.Generic;
using UnityEngine;

/**
 * InputManager handles raw input from the UID. It passes its readings on to a
 * ControlObserver, which translates it into something useful and directs it
 * to its final destination.
 * 
 * The ControlObserver should only be set by the GameManager.
 */
public class InputManager : MonoBehaviour {
	public ControlObserver ControlState { get; set; }
	private Dictionary<KeyCode, KeyState> inputThisFrame;
	private List<KeyCode> buttons;

	void Awake () {
		inputThisFrame = new Dictionary<KeyCode, KeyState> ();
		buttons = new List<KeyCode> ();

		KeyCode[] allButtons = { KeyCode.A, KeyCode.D, KeyCode.LeftArrow, KeyCode.RightArrow,
			KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.Return, KeyCode.Escape };
		
		foreach (KeyCode button in allButtons) {
			inputThisFrame.Add (button, KeyState.None);
			buttons.Add (button);
		}
	}
	
	void Update () {
		foreach (KeyCode key in buttons)
			inputThisFrame[key] = GetKeyState (key);

		ControlState.Notify (inputThisFrame);
	}

	private KeyState GetKeyState(KeyCode key) {
		if (Input.GetKeyDown (key))
			return KeyState.Pressed;
		else if (Input.GetKeyUp (key))
			return KeyState.Released;
		else if (Input.GetKey (key))
			return KeyState.Held;
		else
			return KeyState.None;
	}
}