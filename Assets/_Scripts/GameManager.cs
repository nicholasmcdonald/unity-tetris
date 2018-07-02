using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public Tetromino tetrominoPrefab;
	public GameObject lineCountText;
	public GameObject ui;
	public GameObject controls;

	private Tetromino activeTetromino;
	private Tetromino nextTetromino;
	private GameClock gameClock;
	private TetrominoManager tetrominoManager;
	private InputManager inputManager;
	private UIManager uiManager;
	private int linesCleared = 0;

	void Start() {
		gameClock = GameObject.FindGameObjectWithTag("GameClock").GetComponent<GameClock> ();
		controls = GameObject.FindGameObjectWithTag ("GameController");
		inputManager = controls.GetComponent<InputManager> ();
		inputManager.ControlState = controls.GetComponent<MenuControlState> ();

		tetrominoManager = GetComponent<TetrominoManager> ();
		uiManager = ui.GetComponent<UIManager> ();
		ShowText ();
	}

	public void ActivateAnimationMode() {
		gameClock.BeginAnimation ();
	}

	public void ActivatePlacementMode() {
		gameClock.BeginGravity ();
	}

	public void IncreaseLinesCleared(int count) {
		int oldTotal = linesCleared;
		linesCleared += count;
		lineCountText.GetComponent<TextMesh> ().text = linesCleared.ToString ();

		if ((int)(linesCleared / 10.0f) > (int)(oldTotal / 10.0f))
			gameClock.SpeedUp (linesCleared);
	}
		
	public void Pause() {
		gameClock.IsActive = false;
		uiManager.ShowPauseMenu ();
	}

	public void Resume() {
		gameClock.IsActive = true;
		inputManager.ControlState = controls.GetComponent<GameplayControlState> ();
	}

	public void StartNewGame() {
		StartCoroutine (NewGame ());
	}

	IEnumerator NewGame() {
		// Fix me! This isn't how sound should work
		GetComponent<AudioSource> ().Play ();
		GameObject.FindGameObjectWithTag ("Board").GetComponent<Board> ().PrepareNewGame ();
		lineCountText.GetComponent<TextMesh> ().text = linesCleared.ToString ();

		tetrominoManager.PrepareNewGame ();

		Timer animationTimer = gameClock.GetNewTimer ();
		animationTimer.SetTimeout (3.0f);
		animationTimer.StartCountDown ();
		while (!animationTimer.Ready()) {
			yield return null;
		}
		Destroy (animationTimer.gameObject);

		tetrominoManager.IsActive = true;
		gameClock.PrepareNewGame ();
		inputManager.ControlState = controls.GetComponent<GameplayControlState> ();
	}

	public void GameOver() {
		StartCoroutine (EndGame ());
	}

	IEnumerator EndGame() {
		gameClock.IsActive = false;
		inputManager.ControlState = controls.GetComponent<BlockedControlState> ();
		GameObject.FindGameObjectWithTag ("Finish").GetComponent<AudioSource> ().Play ();

		Timer animationTimer = gameClock.GetNewTimer ();
		animationTimer.SetTimeout (3.0f);
		animationTimer.StartCountDown ();
		while (!animationTimer.Ready ()) {
			yield return null;
		}
		Destroy (animationTimer.gameObject);

		ui.GetComponent<UIManager> ().ShowGameOverMenu ();
		linesCleared = 0;
	}

	void ShowText() {
		GameObject[] text = GameObject.FindGameObjectsWithTag ("Text");
		foreach (GameObject obj in text)
			obj.GetComponent<MeshRenderer> ().sortingLayerName = "Text";
	}
}