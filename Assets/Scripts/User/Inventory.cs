using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Inventory
{
    GenericListJsonLoader<MinionBoughtDef> _minionsBoughts;
    GenericListJsonLoader<string> _squadMinionsOrder;

    string _pathToMinionsSaved;
    string _pathToSquadOrder;

    public Inventory()
    {
        _pathToMinionsSaved = Path.Combine(Application.persistentDataPath, SaveSystem.MINIONS_SAVE_NAME);
        _pathToSquadOrder = Path.Combine(Application.persistentDataPath, SaveSystem.SQUAD_ORDER_SAVE_NAME);

        _minionsBoughts = SaveSystem.Load<GenericListJsonLoader<MinionBoughtDef>>(_pathToMinionsSaved);
        _squadMinionsOrder = SaveSystem.Load<GenericListJsonLoader<string>>(_pathToSquadOrder);

        if (_minionsBoughts == null)
            _minionsBoughts = CreateNewBoughtDefData();

        if (_squadMinionsOrder == null)
            _squadMinionsOrder = CreateNewSquadOrderData();
    }

    /// <summary>
    /// This will add to the inventory a new minion of type 'type'.
    /// And then save it locally
    /// </summary>
    public void AddNewMinionToInventory(MinionType type)
    {
        var minion = CreateMinionDefInstance(type);
        _minionsBoughts.list.Add(minion);
        SaveSystem.Save(_minionsBoughts, _pathToMinionsSaved);
    }

    public void IncrementMinionStat(MinionType type, MinionBoughtDef.StatNames statName)
    {
        var saved = _minionsBoughts.list.FirstOrDefault(i => i.type == type.ToString());
        if(saved != null)
        {
            saved.IncrementLevelToStat(statName);
            SaveSystem.Save(_minionsBoughts, _pathToMinionsSaved);
        }
    }

    public bool IsBought(MinionType type)
    {
        return _minionsBoughts.list.Any(i => i.type == type.ToString());
    }

    public MinionBoughtDef GetMinionBought(MinionType type)
    {
        return _minionsBoughts.list.FirstOrDefault(i => i.type == type.ToString());
    }

    public List<string> GetSquadOrder()
    {
        return _squadMinionsOrder.list;
    }

    public void SetSquadOrderItem(MinionType t)
    {
        if (_squadMinionsOrder.list.Count >= 5) return;

        _squadMinionsOrder.list.Add(t.ToString());
        SaveSystem.Save(_squadMinionsOrder, _pathToSquadOrder);
    }

    GenericListJsonLoader<MinionBoughtDef> CreateNewBoughtDefData()
    {
        var list = new GenericListJsonLoader<MinionBoughtDef>();
        list.list = new List<MinionBoughtDef>();
        var runner = CreateMinionDefInstance(MinionType.Runner);
        list.list.Add(runner);
        SaveSystem.Save(list, _pathToMinionsSaved);

        return list;
    }

    MinionBoughtDef CreateMinionDefInstance(MinionType t)
    {
        var minion = new MinionBoughtDef();
        
        minion.hp = minion.passiveSkill = minion.skill = minion.speed = 1;

        minion.type = t.ToString();
        return minion;
    }

    GenericListJsonLoader<string> CreateNewSquadOrderData()
    {
        GenericListJsonLoader<string> list = new GenericListJsonLoader<string>();
        list.list = new List<string>();
        list.list.Add(MinionType.Runner.ToString());

        SaveSystem.Save(list, _pathToSquadOrder);
        return list;
    }
}
