using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be instantiated only when the user plays the level FOR THE FIRST TIME,
/// then it will update the current data if the user retry the lvl.
/// </summary>
[Serializable]
public class LevelProgress
{
    public int worldId;
    public int levelId;
    public bool won;
    public int starsWon;
    /// <summary>
    /// Amount of minions that the user used to win the level
    /// </summary>
    public int minionAmountUsedToWin;
    public int tries;
    public double firstAttemptAt;
    public double lastAttemptAt;


    public LevelProgress(int lvlId, int worldId)
    {
        levelId = lvlId;
        this.worldId = worldId;
        firstAttemptAt = GameUtils.GetTimestampUTC();

		LevelStarted ();
    }

    /// <summary>
    /// When the user finish the level this func is called (either win or lose)
    /// </summary>
    public void MadeProgress(bool pWin = false, int starsWon = 0)
    {
		//Debug.Log("made progress on lvlprogress");
        won = pWin;
        this.starsWon = starsWon;
    }

	public void LevelStarted()
	{
		//Debug.Log("level started");

		lastAttemptAt = GameUtils.GetTimestampUTC();
		tries++;
	}
}
