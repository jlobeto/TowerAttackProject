using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillsUpgradePanel : MonoBehaviour
{
    public StatUpgradeItem itemGO;

    List<StatUpgradeItem> _list;
    MinionBoughtDef _boughtInfo;
    MinionsStatsCurrencyDef _statsCurrency;
    GenericListJsonLoader<BaseMinionStat> _minionStats;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void SetUpgradeItems(MinionBoughtDef boughtInfo, MinionsStatsCurrencyDef statsCurrency
        , GenericListJsonLoader<BaseMinionStat> minionStats)
    {
        _boughtInfo = boughtInfo;
        _statsCurrency = statsCurrency;
        _minionStats = minionStats;

        if (_list == null || _list.Count == 0)
            GenerateItemsFromScratch();
        else
            ReloadItems();
    }

    void GenerateItemsFromScratch()
    {
        var hasHPStat = _statsCurrency.baseHPCurrencyValue != 0;
        
        var item = Instantiate<StatUpgradeItem>(itemGO, transform);
        var name = hasHPStat ? "HP :" : "Passive :";
        float curr = _minionStats.list.First(i => i.levelId == _boughtInfo.hp).hp;
        float next = _minionStats.list.First(i => i.levelId == _boughtInfo.hp+1).hp;
        item.SetItem(_boughtInfo.hp, curr, next, name);

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        name = "SPD :";
        curr = _minionStats.list.First(i => i.levelId == _boughtInfo.speed).speed;
        next = _minionStats.list.First(i => i.levelId == _boughtInfo.speed+1).speed;
        item.SetItem(_boughtInfo.speed, curr, next, name);

        if (_boughtInfo.type == MinionType.Dove.ToString())
            return;

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        name = "SKILL :";

        if (_boughtInfo.type == MinionType.Runner.ToString())
        {
            curr = _minionStats.list.First(i => i.levelId == _boughtInfo.skill).skillDeltaSpeed;
            next = _minionStats.list.First(i => i.levelId == _boughtInfo.skill + 1).skillDeltaSpeed;
        }
        else if (_boughtInfo.type == MinionType.Tank.ToString())
        {
            curr = _minionStats.list.First(i => i.levelId == _boughtInfo.skill).skillArea;
            next = _minionStats.list.First(i => i.levelId == _boughtInfo.skill + 1).skillArea;
        }

        item.SetItem(_boughtInfo.speed, curr, next, name);
    }

    void ReloadItems()
    {

    }

}
