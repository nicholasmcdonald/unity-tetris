using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour {
	private float time = 0.0f;
	private float timeout = 0.0f;
	private bool active = false;

	void Update() {
		if (active)
			time += Time.deltaTime;
	}
		
	public void StartCountDown() {
		active = true;
	}

	public void Stop () {
		active = false;
		Reset ();
	}

	public void Lap () {
		Reset ();
	}

	public void SetTimeout(float timeout) {
		this.timeout = timeout;
	}

	public bool Ready() {
		return time >= timeout;
	}

	private void Reset() {
		time = 0.0f;
	}
}