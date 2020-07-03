using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelProgressManager
{

    GenericListJsonLoader<LevelProgress> _lvlProgressList;
    GameManager _gameManager;
    public LevelProgressManager(GameManager gameManager)
    {

        _lvlProgressList = SaveSystem.Load<GenericListJsonLoader<LevelProgress>>(SaveSystem.LEVEL_PROGRESS_SAVE_NAME);
        if (_lvlProgressList == null || _lvlProgressList.list == null)
        {
            _lvlProgressList = new GenericListJsonLoader<LevelProgress>();
            _lvlProgressList.list = new List<LevelProgress>();//have to init this because other scripts are using it.
            Save();
        }
            
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
        {
            var result = progress.MadeProgress(won, stars);
            if(result)
                Save();
        }
            
    }

	public LevelProgress GetProgress(int lvlId)
	{
		foreach (var item in _lvlProgressList.list)
		{
			if (item.levelId == lvlId)
				return item;
		}

		return null;
	}
    

    public int GetStarsAccumulated()
    {
        int amount = 0;
        foreach (var item in _lvlProgressList.list)
        {
            amount += item.starsWon;
        }

        return amount;
    }

    public bool AreLevelsWonByWorld(int worldId)
    {
		var levelProgressCountByWorld = 0;
        foreach (var item in _lvlProgressList.list)
        {
            if(item.worldId == worldId)
            {
				levelProgressCountByWorld++;
                if (!item.won)
                    return false;
            }
        }
		var levelsInWorld = _gameManager.LevelInfoLoader.LevelInfoList.list.Count (i => i.worldId == worldId);

		return levelsInWorld == levelProgressCountByWorld;
    }

    public void ForceWinAllLevels()
    {
        _lvlProgressList = new GenericListJsonLoader<LevelProgress>();
        _lvlProgressList.list = new List<LevelProgress>();
        foreach (var item in _gameManager.LevelInfoLoader.LevelInfoList.list)
        {
            LevelStarted(item.id);
            LevelEnded(item.id, true, 3);
        }
    }


    /// <summary>
    /// Get Current Level.
    /// </summary>
    public int GetCurrentUserLevel()
    {
        var lastestSaved = _lvlProgressList.list.Count > 0 ? _lvlProgressList.list.Last() : null;
        if (lastestSaved == null)
            return 1;

        if (!lastestSaved.won)
            return lastestSaved.levelId;
        else
            return lastestSaved.levelId + 1;
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
            _lvlProgressList.list.Add(existed);

            Save();
        }
        else
        {
        	existed.LevelStarted();
            Save();
        }

        return existed;
    }

    void Save()
    {
        SaveSystem.Save(_lvlProgressList, SaveSystem.LEVEL_PROGRESS_SAVE_NAME);
    }
}
