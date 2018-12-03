using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressManager
{
    List<LevelProgress> _lvlProgressList;
    GameManager _gameManager;

    public LevelProgressManager(GameManager gameManager)
    {
        _lvlProgressList = new List<LevelProgress>();
        _gameManager = gameManager;
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
    

    public int GetStarsAccumulated()
    {
        int amount = 0;
        foreach (var item in _lvlProgressList)
        {
            amount += item.StarsWon;
        }

        return amount;
    }

    public bool AreLevelsWonByWorld(int worldId)
    {
        foreach (var item in _lvlProgressList)
        {
            if(item.WorldId == worldId)
            {
                if (!item.Won)
                    return false;
            }
        }
        return true;
    }

    public void ForceWinAllLevels()
    {
        _lvlProgressList = new List<LevelProgress>();
        foreach (var item in _gameManager.LevelInfoLoader.LevelInfoList.list)
        {
            LevelStarted(item.id);
            LevelEnded(item.id, true, 3);
        }
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
            var world = _gameManager.LevelInfoLoader.LevelInfoList.list.FirstOrDefault(i => i.id == lvlId).worldId;
            existed = new LevelProgress(lvlId, world);
            _lvlProgressList.Add(existed);
            //Debug.Log("creating new lvl progress");
        }
        else
        {
        	existed.LevelStarted();
        }

        return existed;
    }
}
