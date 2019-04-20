using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private TetrominoManager tetrominoManager;
	private Block[,] grid;
	private const int GRID_HEIGHT = 22;
	private const int GRID_WIDTH = 10;
	private List<int> completedRows;

	void Start () {
		tetrominoManager = GameObject.FindGameObjectWithTag ("Session").GetComponent<TetrominoManager> ();
        grid = new Block[GRID_HEIGHT, GRID_WIDTH];
        foreach (Transform block in transform.Find("Blocks"))
            Destroy(block.gameObject);
    }

	/* Gather occupancy information for movement target */
	public bool[,] CreateTestGrid(Vector2 coordinates) {
		// Try to read points on grid
		bool[,] testGrid = new bool[4,4];

		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				// If out of bounds, mark as filled; otherwise check for occupancy
				if (coordinates.x + j < 0 || coordinates.x + j >= GRID_WIDTH
					|| coordinates.y + i < 2 || coordinates.y + i >= GRID_HEIGHT) {
					testGrid [i, j] = true;
				} else {
					int x = (int)coordinates.x + j;
					int y = (int)coordinates.y + i;
					testGrid [i, j] = (grid [y,x] != null);
				}
			}
		}

		return testGrid;
	}

	public void Fix(Vector2 coordinates, Block block) {
		int x = (int)coordinates.x;
		int y = (int)coordinates.y;
		grid [y, x] = block;
		block.gameObject.transform.parent = transform.Find("Blocks");
	}

	public void CompleteLines() {
		FindCompleteRows ();
		FlashBlocks ();
		if (completedRows.Count > 0) {
			//gameManager.ActivateAnimationMode ();
		} else {
			//gameManager.ActivatePlacementMode ();
		}
	}

	/* Called by the GameClock when in animation mode */
	void Animate() {
		DestroyRows ();
		DropFloatingBlocks ();
		CompleteLines ();
	}

	public void DestroyRows() {
		// gameSession.ScoreKeeper.RegisterScore (completedRows.Count);
        // Tell TetrominoManager instead
		foreach (int row in completedRows) {
			for (int column = 0; column < GRID_WIDTH; column++) {
				Destroy (grid [row, column].gameObject);
				grid [row, column] = null;
			}
		}
	}

	private void FindCompleteRows() {
		completedRows = new List<int>();
		for (int row = 0; row < GRID_HEIGHT; row++) {
			bool rowHasNulls = false;

			for (int column = 0; column < GRID_WIDTH; column++) {
				if (grid [row, column] == null) {
					rowHasNulls = true;
					break;
				}
			}

			if (!rowHasNulls) {
				completedRows.Add (row);
			} 
		}
	}

	private void FlashBlocks() {
		foreach (int row in completedRows) {
			for (int column = 0; column < GRID_WIDTH; column++) {
				grid [row, column].GetComponent<SpriteRenderer> ().color = Color.white;
			}
		}
	}

	private void DropFloatingBlocks() {
		int[] lowestFreeCell = new int[GRID_WIDTH];
		for (int column = 0; column < GRID_WIDTH; column++) {
			lowestFreeCell [column] = -1;
		}

		// Iterate from bottom to top
		for (int row = GRID_HEIGHT - 1; row >= 0; row--) {
			for (int column = 0; column < GRID_WIDTH; column++) {
				// If lowest free is set, watch for blocks
				// If lowest free is not set, watch for empties
				if (lowestFreeCell [column] == -1 && grid [row, column] == null) {
					lowestFreeCell [column] = row;
				} else if (lowestFreeCell [column] >= 0 && grid [row, column] != null) {
					grid [lowestFreeCell [column], column] = grid [row, column];

					float difference = row - lowestFreeCell [column];
					grid [row, column].transform.Translate (0.0f, difference, 0.0f);
					grid [row, column] = null;

					lowestFreeCell [column]--;
				}
			}
		}
	}
}