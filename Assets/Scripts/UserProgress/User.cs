using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will contain all the data to be stored, or the be accessed by the entire game.
/// Containing the progress, the inventory data and that shits.
/// </summary>
public class User
{
    /// <summary>
    /// Set as the UserId at the moment...
    /// </summary>
    string _deviceId;

    LevelProgressManager _levelProgressManager;

    public User()
    {
        _deviceId = SystemInfo.deviceUniqueIdentifier;

        _levelProgressManager = new LevelProgressManager();
    }

    public void LevelStarted(int lvlId)
    {
        _levelProgressManager.LevelStarted(lvlId);
    }

    public void LevelEnded(int lvlId, bool won = false, int stars = 0)
    {
        _levelProgressManager.LevelEnded(lvlId, won, stars);
    }
}
