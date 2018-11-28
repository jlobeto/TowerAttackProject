using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will be instantiated only when the user plays the level FOR THE FIRST TIME,
/// then it will update the current data if the user retry the lvl.
/// </summary>
public class LevelProgress
{
    public int LevelId { get { return _levelId; } }
    public int StarsWon { get { return _starsWon; } }
    public bool Won { get { return _won; } }

    int _levelId;
    bool _won;
    int _starsWon;
    /// <summary>
    /// Amount of minions that the user used to win the level
    /// </summary>
    int _minionAmountUsedToWin;
    int _tries;
    double _firstAttemptAt;
    double _lastAttemptAt;


    public LevelProgress(int lvlId)
    {
        _levelId = lvlId;
        _firstAttemptAt = GameUtils.GetTimestampUTC();

		LevelStarted ();
    }

    /// <summary>
    /// When the user finish the level this func is called (either win or lose)
    /// </summary>
    public void MadeProgress(bool pWin = false, int starsWon = 0)
    {
		Debug.Log("made progress on lvlprogress");
        _won = pWin;
        _starsWon = starsWon;
    }

	public void LevelStarted()
	{
		Debug.Log("level started");

		_lastAttemptAt = GameUtils.GetTimestampUTC();
		_tries++;
	}
}
