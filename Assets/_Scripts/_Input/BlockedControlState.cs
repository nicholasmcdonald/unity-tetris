using System.Collections.Generic;
using UnityEngine;

public class BlockedControlState : MonoBehaviour, ControlObserver {
	public void Notify(Dictionary<KeyCode, KeyState> inputs) {
		// Do nothing!
	}
}
