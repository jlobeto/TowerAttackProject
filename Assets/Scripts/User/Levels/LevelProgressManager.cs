using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressManager
{
    List<LevelProgress> _lvlProgressList;

    public LevelProgressManager()
    {
        _lvlProgressList = new List<LevelProgress>();
    }

    public void LevelStarted(int lvlId)
    {
        SetLevelProgress(lvlId);
    }

    public void LevelEnded(int lvlId, bool won , int stars )
    {
        var progress = GetProgress(lvlId);

        if (progress != null)
            progress.MadeProgress(won, stars);
    }

	public LevelProgress GetProgress(int lvlId)
	{
		foreach (var item in _lvlProgressList)
		{
			if (item.LevelId == lvlId)
				return item;
		}

		return null;
	}

    /*
     * ////////////////////////////////////////////////
     *              P R I V A T E S
     * ////////////////////////////////////////////////
     */


    LevelProgress SetLevelProgress(int lvlId)
    {
        var existed = GetProgress(lvlId);

        if (existed == null)
        {
            existed = new LevelProgress(lvlId);
            _lvlProgressList.Add(existed);
            Debug.Log("creating new lvl progress");
        }
        else
        {
        	existed.LevelStarted();
        }

        return existed;
    }
}
