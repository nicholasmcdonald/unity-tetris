using System.Collections.Generic;
using UnityEngine;

public interface ControlObserver {
	void Notify (Dictionary<KeyCode, KeyState> inputs);
}