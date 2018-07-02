using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoManager : MonoBehaviour {
	public bool IsActive { get; set; }
	public Tetromino tetrominoPrefab;

	private Tetromino activeTetromino;
	private Tetromino nextTetromino;
	private bool fastDropBlocked = false;

	public void PrepareNewGame() {
		ClearTetrominoes ();
		SpawnNext ();
	}

	public void MoveLaterally(KeyCode key) {
		if (key == KeyCode.LeftArrow)
			activeTetromino.MoveLaterally (Vector2.left);
		else if (key == KeyCode.RightArrow)
			activeTetromino.MoveLaterally (Vector2.right);
	}

	public void Rotate(KeyCode key) {
		activeTetromino.Rotate (key);
	}
		
	public void FastDrop() {
		if (!fastDropBlocked)
			activeTetromino.MoveDown ();
	}

	public void UnblockFastDrop() {
		fastDropBlocked = false;
	}

	public void Gravity() {
		activeTetromino.MoveDown ();
	}
		
	void Update() {
		if (IsActive) {
			if (activeTetromino == null) {
				ActivateWaitingTetromino ();
				SpawnNext ();
			}
		}
	}

	void SpawnNext() {
		nextTetromino = Instantiate (tetrominoPrefab, transform);
		nextTetromino.pattern = GetRandomPattern ();
		fastDropBlocked = true;
	}

	Pattern GetRandomPattern() {
		Array patterns = Enum.GetValues (typeof(Pattern));
		System.Random random = new System.Random ();
		return (Pattern)patterns.GetValue (random.Next (patterns.Length - 1));
	}

	void ActivateWaitingTetromino() {
		activeTetromino = nextTetromino;
		if (nextTetromino.Activate ())
			fastDropBlocked = true;
		else {
			IsActive = false;
			GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager> ().GameOver ();
		}
	}

	void ClearTetrominoes() {
		if (activeTetromino != null)
			Destroy (activeTetromino.gameObject);
		if (nextTetromino != null)
			Destroy (nextTetromino.gameObject);
	}
}