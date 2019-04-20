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
        CheckForLevelUp(score);
		this.linesCleared += score.LinesCleared;
        this.score += CalculateScore(score);
        this.quadStreak = CheckForQuadStreak(score.LinesCleared);
	}

	private int ScoreLines(Score score) {
		int[] basePoints = { 0, 40, 100, 300, 1200 };
		int tempScore = basePoints [score.LinesCleared] * (level + 1);

		if (score.LinesCleared == 4 && quadStreak)
			tempScore *= 2;

		return tempScore;
	}

	private bool CheckForQuadStreak(int linesCleared) {
		return linesCleared == 4;
	}

    private int CalculateScore(Score score)
    {
        int runningTotal = 0;
        runningTotal += ScoreLines(score);
        runningTotal += ScoreSoftDrop(score);
        return runningTotal;
    }

    private int ScoreSoftDrop(Score score)
    {
        return score.SoftDropTiles;
    }

    private void CheckForLevelUp(Score score)
    {
        int oldLinesCleared = this.linesCleared;
        int newLinesCleared = this.linesCleared + score.LinesCleared;
        if ((int)(newLinesCleared / 10.0f) > (int)(oldLinesCleared / 10.0f))
            GameObject.FindGameObjectWithTag("Session").GetComponent<GameSession>().LevelUp();
    }
}
