using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject sessionPrefab;
	public GameObject lineCountText;
	public GameObject ui;
	public GameObject controls;

    public GameSession Session { get { return session; } }
	private InputManager inputManager;
	private UIManager uiManager;
	private int linesCleared = 0;
    private GameSession session;

	void Start() {
		controls = GameObject.FindGameObjectWithTag ("GameController");
		inputManager = controls.GetComponent<InputManager> ();
		inputManager.ControlState = controls.GetComponent<MenuControlState> ();
		uiManager = ui.GetComponent<UIManager> ();
	}
		
	public void Pause() {
		uiManager.ShowPauseMenu ();
	}

	public void Resume() {
		inputManager.ControlState = controls.GetComponent<GameplayControlState> ();
	}

	public void StartNewGame() {
		StartCoroutine (NewGame ());
	}

	IEnumerator NewGame() {
		// Fix me! This isn't how sound should work
		GetComponent<AudioSource> ().Play ();
		lineCountText.GetComponent<TextMesh> ().text = linesCleared.ToString ();
        // Fix the above!

        GameObject newSession = Instantiate(sessionPrefab);
        session = newSession.GetComponent<GameSession>();

        float timer = 0.0f;
		while (timer < 3.0f) {
            timer += Time.deltaTime;
			yield return null;
		}

		inputManager.ControlState = controls.GetComponent<GameplayControlState> ();
        session.Begin();
	}

	public void GameOver() {
		StartCoroutine (EndGame ());
	}

	IEnumerator EndGame() {
		inputManager.ControlState = controls.GetComponent<BlockedControlState> ();
		GameObject.FindGameObjectWithTag ("Finish").GetComponent<AudioSource> ().Play ();

        float timer = 0.0f;
        while (timer < 3.0f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        ui.GetComponent<UIManager> ().ShowGameOverMenu ();
		linesCleared = 0;
	}
}