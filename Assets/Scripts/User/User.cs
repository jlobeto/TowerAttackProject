using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This will contain all the data to be stored, or the be accessed by the entire game.
/// Containing the progress, the inventory data and that shits.
/// </summary>
public class User
{
    int _currency = 100;

    /// <summary>
    /// Set as the UserId at the moment...
    /// </summary>
    string _deviceId;
    LevelProgressManager _levelProgressManager;
    GameManager _gameManager;

	public LevelProgressManager LevelProgressManager { get { return _levelProgressManager; } }
    public int Currency { get { return _currency; } }

    public User(GameManager gameManager)
    {
        _deviceId = SystemInfo.deviceUniqueIdentifier;
        _gameManager = gameManager;
        _levelProgressManager = new LevelProgressManager(_gameManager);
    }

    public void LevelStarted(int lvlId)
    {
        _levelProgressManager.LevelStarted(lvlId);
    }

    public void LevelEnded(int lvlId, bool won = false, int stars = 0)
    {
        _levelProgressManager.LevelEnded(lvlId, won, stars);
        if(stars > 0)
        {
            var info = _gameManager.LevelInfoLoader.LevelInfoList.list.First(i => i.id == lvlId);
            _currency += info.currencyGainedByObjectives[stars - 1];
        }
    }


}
