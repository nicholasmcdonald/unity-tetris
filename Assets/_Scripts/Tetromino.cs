using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {
	private static int spawnCount = 0;

	public Block blockPrefab;
	public Pattern pattern;

	private const int BLOCKS_PER_TETROMINO = 4;
	private const int START_Y = 19;
	private const int START_X = 5;

	private Board board;
	private List<Block> blocks;
	private int rotation = 0;
	private bool isInWaiting = true;
	private Vector2 gridAlignmentOffset;
	/* The X,Y for the top left cell in the Tetromino square */
	private Vector2 Coordinates {
		get {
			int x = (int)transform.position.x - 2;
			int y = 22 - ((int)transform.position.y + 2);
			 
			return new Vector2 (x, y);
		}
	}

	/**
	 * Move the Tetromino into the main alley and allow it to be affected by
	 * gravity, etc.
	 */
	public bool Activate() {
		this.transform.position = new Vector2 (START_X, START_Y);
		isInWaiting = false;
		return ValidateSpawn ();
	}

	public void Rotate(KeyCode key) {
		int spinAmount;
		if (key == KeyCode.A)
			spinAmount = 1;
		else if (key == KeyCode.D)
			spinAmount = 3;
		else
			return;

		int newRotation = (rotation + spinAmount) % 4;
		if (TryRotate (newRotation)) {
			rotation = newRotation;
			Draw ();
		}
	}

	public void MoveLaterally(Vector2 direction) {
		if (TryMove(direction))
			transform.Translate (direction);
	}

	public void MoveDown() {
		// If the Tetromino is at the bottom, fix it to the board and check for lines
		if (!isInWaiting && !IsStuck()) {
			FixToBoard ();
		}
	}

	void Start () {
		// Track number of spawns, for, whatever reason
		spawnCount++;

		// Grab a reference to the game board
		board = GameObject.FindGameObjectWithTag ("Board").GetComponent<Board> ();

		// Create all the blocks associated with the Tetromino
		Color color = Block.GetRandomColor ();
		blocks = new List<Block> ();
		for (int i = 0; i < BLOCKS_PER_TETROMINO; i++) {
			blocks.Add (Instantiate (blockPrefab, transform));
			blocks [i].GetComponent<SpriteRenderer> ().color = color;
		}

		// Get top-left corner of tetromino as origin and shift so block corner
		// lines up with it
		gridAlignmentOffset = new Vector2(-1.5f, 1.5f);

		ShowInWaitingArea ();
	}

	/**
	 * Position the Blocks within the Tetromino grid after a rotation etc. 
	 */
	void Draw() {
		// Retrieve the pattern for drawing
		bool[,] patternGrid = BlockMatrix.PatternFor(pattern, rotation);
		int count = 0;

		// Pass over the grid, placing Blocks whenever a slot is occupied
		for (int row = 0; row < BlockMatrix.GRID_SIZE; row++) {
			for (int column = 0; column < BlockMatrix.GRID_SIZE; column++) {
				// Don't forget: matrices store values (y,x) ...
				if (patternGrid[column,row]) {
					// ... but screen coordinates are (x,y)!
					Vector2 position = gridAlignmentOffset + new Vector2(row, 0 - column);
					blocks [count++].transform.localPosition = position;
				}
			}
		}
	}

	/* Determine the world coordinates for the Blocks in the Tetromino and 
	 * let the Board keep the Block reference in that cell.
	 */
	void FixToBoard() {
		bool[,] grid = BlockMatrix.PatternFor(pattern, rotation);

		for (int i = 0; i < BlockMatrix.GRID_SIZE; i++) {
			for (int j = 0; j < BlockMatrix.GRID_SIZE; j++) {
				if (grid [j, i]) {
					int x = (int)Coordinates.x + i;
					int y = (int)Coordinates.y + j;
					Vector2 target = new Vector2 (x, y);

					Block transfer = blocks [0];
					blocks.RemoveAt (0);

					board.Fix (target, transfer);
				}
			}
		}

		Destroy (gameObject);
		board.CompleteLines ();
	}

	bool TryMove(Vector2 direction) {
		return ValidateMove (direction, rotation);
	}

	bool TryRotate(int rotation) {
		return ValidateMove (Vector2.zero, rotation);
	}

	/* Attempts to move the Tetromino down one cell. If it cannot, the Tetromino
	 * needs to be fixed to the board.
	 * NOTE that this is called every tick, not every frame
	 * 
	 * @return	True if the Tetromino was moved
	 */
	bool IsStuck() {
		if (TryMove (Vector2.up)) {
			transform.Translate (Vector3.down);
			return true;
		} else {
			return false;
		}
	}

	/* Take a reading from the board and see if anything would block the Tetromino
	 * from moving to a new location.
	 * 
	 * @param direction	Target offset from current location (zero for rotation)
	 * @param rotation	Desired rotation in new position
	 * @return 	False if move is impossible
	 */
	bool ValidateMove(Vector2 direction, int rotation) {
		// Specify the location the Tetromino is attempting to move to
		Vector2 target = Coordinates + direction;

		// Grab the cells at that location and a copy of the Tetromino pattern
		bool[,] testGrid = board.CreateTestGrid (target);
		bool[,] patternGrid = BlockMatrix.PatternFor(pattern, rotation);

		// Ensure there are gaps in the board whenever there are blocks in the pattern
		for (int row = 0; row < BlockMatrix.GRID_SIZE; row++) {
			for (int column = 0; column < BlockMatrix.GRID_SIZE; column++) {
				// If the testgrid is already occupied where the tetromino is trying to go, fail
				if (testGrid [row, column] && patternGrid [row, column]) {
					return false;
				}
			}
		}

		// Looks like we're good to go!
		return true;
	}

	bool ValidateSpawn() {
		return ValidateMove (new Vector2 (0.0f, 0.0f), rotation);
	}

	/**
	 * Spawn the upcoming Tetromino in the small side grid, where it stays
	 * until it becomes active.
	 */
	void ShowInWaitingArea() {
		Vector2 targetPosition = board.transform.Find ("next_tiled").transform.position;
		this.transform.position = targetPosition;
		Draw ();
	}
}