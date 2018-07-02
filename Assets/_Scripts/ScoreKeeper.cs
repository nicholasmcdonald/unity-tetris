using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
	public int LinesCleared { get { return linesCleared; } }
	public int Score { get { return score; } }

	private int linesCleared = 0;
	private int score = 0;
	private bool quadStreak = false;
	private int level = 0;

	public void RegisterNewScore(Score score) {
		this.linesCleared += score.LinesCleared;
		this.score += ScoreLines (score.LinesCleared);
		this.score += ScoreSoftDrop (score.SoftDropTiles);
		this.quadStreak = CheckForQuadStreak (score.LinesCleared);
	}

	private int ScoreLines(int linesCleared) {
		int[] basePoints = { 0, 40, 100, 300, 1200 };
		int tempScore = basePoints [linesCleared] * (level + 1);

		if (linesCleared == 4 && quadStreak)
			tempScore *= 2;

		return tempScore;
	}

	private int ScoreSoftDrop(int tilesDropped) {
		return tilesDropped;
	}

	private bool CheckForQuadStreak(int linesCleared) {
		return linesCleared == 4;
	}
}
