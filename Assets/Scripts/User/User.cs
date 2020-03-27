using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO;

/// <summary>
/// This will contain all the data to be stored, or the be accessed by the entire game.
/// Containing the progress, the inventory data and that shits.
/// </summary>
public class User
{
    int _currency = 30;
    Inventory _inventory;
    /// <summary>
    /// Set as the UserId at the moment...
    /// </summary>
    string _deviceId;
    LevelProgressManager _levelProgressManager;
    GameManager _gameManager;

	public LevelProgressManager LevelProgressManager { get { return _levelProgressManager; } }

    public int Currency
    {
        get { return _currency; }
        set
        {
            var coinsSavedPath = Path.Combine(Application.persistentDataPath, SaveSystem.CURRENCY_SAVE_NAME);
            _currency = value;
            var coinsSaved = SaveSystem.Load<GenericListJsonLoader<int>>(coinsSavedPath);
            coinsSaved.list[0] = _currency;
            SaveSystem.Save(coinsSaved, coinsSavedPath);
        }
    }

    public Action OnMinionBought = delegate { };

    public User(GameManager gameManager)
    {
        _deviceId = SystemInfo.deviceUniqueIdentifier;
        _gameManager = gameManager;
        _levelProgressManager = new LevelProgressManager(_gameManager);
        _inventory = new Inventory();

        var coinsSavedPath = Path.Combine(Application.persistentDataPath, SaveSystem.CURRENCY_SAVE_NAME);
        var coinsSaved = SaveSystem.Load<GenericListJsonLoader<int>>(coinsSavedPath);

        if (coinsSaved == null)
        {
            var genList = new GenericListJsonLoader<int>
            {
                list = new List<int>() { _currency }
            };
            SaveSystem.Save(genList, coinsSavedPath);
        }
        else if (coinsSaved.list.Count > 0)
            Currency = coinsSaved.list[0];
        else
            Currency = 1;
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
            Currency += info.currencyGainedByObjectives[stars - 1];
        }
    }

    public bool MinionIsInInvetory(MinionType t)
    {
        return _inventory.IsBought(t);
    }

    public void BuyMinion(MinionType t, int valueOfMinion)
    {
        Currency -= valueOfMinion;
        _inventory.AddNewMinionToInventory(t);
        OnMinionBought();
    }

    public void BuyMinionStat(MinionType type, MinionBoughtDef.StatNames statName, int price)
    {
        Currency -= price;
        _inventory.IncrementMinionStat(type, statName);
    }

    public MinionBoughtDef GetMinionBought(MinionType t)
    {
        return _inventory.GetMinionBought(t);
    }

    public List<string> GetSquadMinionsOrder()
    {
        return _inventory.GetSquadOrder();
    }

    public void SetSquadMinionItem(MinionType t)
    {
        _inventory.SetSquadOrderItem(t);
    }

    public void DeleteSquadMinionItem(MinionType t)
    {
        _inventory.DeleteSquadOrderItem(t);
    }
}
