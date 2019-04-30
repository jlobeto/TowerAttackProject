using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelProgressManager
{
    const string SAVE_FILE = "LevelProgressData.txt";

    GenericListJsonLoader<LevelProgress> _lvlProgressList;
    GameManager _gameManager;
    string _pathToSave;
    public LevelProgressManager(GameManager gameManager)
    {
        _pathToSave = Path.Combine(Application.persistentDataPath, SAVE_FILE);

        _lvlProgressList = SaveSystem.Load<GenericListJsonLoader<LevelProgress>>(_pathToSave);
        if (_lvlProgressList == null)
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
            progress.MadeProgress(won, stars);
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
        SaveSystem.Save(_lvlProgressList, _pathToSave);
    }
}
