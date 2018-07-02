using System.Collections;
using System.Collections.Generic;

/* A BlockMatrix is a representation of a tetromino as a 4x4 grid with 
 * occupied squares labelled '1'. The matrix has been compressed into a
 * string with each set of four digits representing a row, but is passed
 * to a Tetromino object in matrix form.
 * 
 * It was easier than typing out the matrices by hand.
 */
public static class BlockMatrix {
	public const int GRID_SIZE = 4;

	private static Dictionary<Pattern, List<string>> allMatrices 
		= new Dictionary<Pattern, List<string>> 
	{
		{ Pattern.I, new List<string> {
				"0000111100000000", "0010001000100010", "0000111100000000", "0010001000100010"
			}
		},
		{ Pattern.T, new List<string> {
				"0000111001000000", "0100110001000000", "0000010011100000", "0100011001000000"
			}
		},
		{ Pattern.L, new List<string> {
				"0000111010000000", "1100010001000000", "0000001011100000", "0100010001100000"
			}
		},
		{ Pattern.J, new List<string> {
				"0000111000100000", "0100010011000000", "0000100011100000", "0110010001000000"
			}
		},
		{ Pattern.S, new List<string> {
				"0000011011000000", "1000110001000000", "0000011011000000", "1000110001000000"
			}
		},
		{ Pattern.Z, new List<string> {
				"0000110001100000", "0010011001000000", "0000110001100000", "0010011001000000"
			}
		},
		{ Pattern.O, new List<string> {
				"0000011001100000", "0000011001100000", "0000011001100000", "0000011001100000"
			}
		}
	};

	/* Convert the BlockMatrix string into its matrix form. Each set
	 * of four symbols represents a row of the matrix */
	public static bool[,] PatternFor(Pattern pattern, int rotation) {
		string matrix = allMatrices [pattern][rotation];
		bool[,] grid = new bool[GRID_SIZE, GRID_SIZE];

		// For each row
		for (int i = 0; i < GRID_SIZE; i++) {
			// We read in four at a time
			int offset = i * GRID_SIZE;

			// Assign the value of each column entry to the grid
			for (int j = 0; j < GRID_SIZE; j++) {
				bool occupied = (matrix [offset + j] == '1');
				grid[i, j] = occupied;
			}
		}

		return grid;
	}
}