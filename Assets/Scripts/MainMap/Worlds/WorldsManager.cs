using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manage the data in the worlds.
/// </summary>
public class WorldsManager
{
    GenericListJsonLoader<WorldItem> _worldsJson;
    User _user;

    public WorldsManager(User u)
    {
        _user = u;
        _worldsJson = GameUtils.LoadConfig<GenericListJsonLoader<WorldItem>>("WorldsInfo.json");
    }

    public List<int> GetUnlockWorlds()
    {
        var starsWon = _user.LevelProgressManager.GetStarsAccumulated();
        List<int> worlds = new List<int>();

        foreach (var item in _worldsJson.list)
        {
            if(starsWon >= item.amountToUnlock)
            {
                worlds.Add(item.worldId);
            }
        }

        return worlds;
    }

    public int GetStarsLeftAmount(int world)
    {
        var starsWon = _user.LevelProgressManager.GetStarsAccumulated();
        var neededToUnlock = _worldsJson.list.FirstOrDefault(i => i.worldId == world).amountToUnlock;
        var result = neededToUnlock - starsWon;

        if (result > 0)
            return result;

        return 0;
    }
}
