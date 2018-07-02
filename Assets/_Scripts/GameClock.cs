using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClock : MonoBehaviour {
	public Timer timerPrefab;

	public bool IsActive { get; set; }

	private int timer = 0;
	private int tickRate = 60;
	private const float fastTickLength = 1.0f;
	private const float ANIMATION_TICK = 20.0f;
	private bool useAnimationTimer = false;

	void Start() {
		IsActive = false;
	}

	void Update () {		
		if (IsActive) {
			timer++;

			if (!useAnimationTimer) {
				if (timer >= tickRate) {
					timer = 0;
					BroadcastMessage ("Gravity");
				}
			} else {
				if (timer >= ANIMATION_TICK) {
					timer = 0;
					BroadcastMessage ("Animate");
				}
			}
		}
	}

	public void PrepareNewGame() {
		BeginGravity ();
		ResetTickRate ();
		IsActive = true;
	}

	public void BeginAnimation() {
		useAnimationTimer = true;
		timer = 0;
	}

	public void BeginGravity() {
		useAnimationTimer = false;
		timer = 0;
	}

	public void SpeedUp(int linesCleared) {
		if (linesCleared <= 100)
			tickRate -= 5;
		else if (linesCleared <= 200)
			tickRate -= 1;
		else
			tickRate -= 0;
	}

	public void ResetTickRate() {
		tickRate = 60;
	}

	public Timer GetNewTimer() {
		return Instantiate (timerPrefab);
	}
}