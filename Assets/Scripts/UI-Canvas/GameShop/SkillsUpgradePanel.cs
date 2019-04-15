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

    public void HideAllStats()
    {
        if (_list == null) return;

        foreach (var item in _list)
        {
            item.gameObject.SetActive(false);
        }
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

        BaseMinionStat currStat = _minionStats.list.First(i => i.levelId == _boughtInfo.hp);
        BaseMinionStat nextStat = _minionStats.list.First(i => i.levelId == _boughtInfo.hp + 1);

        var item = Instantiate<StatUpgradeItem>(itemGO, transform);
        var name = hasHPStat ? "HP :" : "Passive :";
        float curr = currStat.hp;
        float next = nextStat.hp;

        if (_boughtInfo.type == MinionType.Healer.ToString())
        {
            curr = _minionStats.list.First(i => i.levelId == _boughtInfo.passiveSkill).healPerSecond;
            next = _minionStats.list.First(i => i.levelId == _boughtInfo.passiveSkill + 1).healPerSecond;
        }
        else if(_boughtInfo.type == MinionType.WarScreamer.ToString())
        {
            curr = _minionStats.list.First(i => i.levelId == _boughtInfo.passiveSkill).passiveSpeedDelta;
            next = _minionStats.list.First(i => i.levelId == _boughtInfo.passiveSkill + 1).passiveSpeedDelta;
        }

        
        item.SetItem(_boughtInfo.hp, curr, next, name);

        _list = new List<StatUpgradeItem>();
        _list.Add(item);

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        name = "SPD :";
        curr = _minionStats.list.First(i => i.levelId == _boughtInfo.speed).speed;
        next = _minionStats.list.First(i => i.levelId == _boughtInfo.speed+1).speed;
        item.SetItem(_boughtInfo.speed, curr, next, name);
        _list.Add(item);

        if (_boughtInfo.type == MinionType.Dove.ToString())
            return;

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        name = "SKILL :";

        BaseMinionStat skillStat = _minionStats.list.First(i => i.levelId == _boughtInfo.skill);
        BaseMinionStat skillStatNext = _minionStats.list.First(i => i.levelId == _boughtInfo.skill + 1);

        if (_boughtInfo.type == MinionType.Runner.ToString())
        {
            curr = skillStat.skillDeltaSpeed;
            next = skillStatNext.skillDeltaSpeed;
        }
        else if (_boughtInfo.type == MinionType.Tank.ToString())
        {
            curr = skillStat.skillArea;
            next = skillStatNext.skillArea;
        }
        else if (_boughtInfo.type == MinionType.Zeppelin.ToString())
        {
            curr = skillStat.miniZeppelinStat.hitsToDie;
            next = skillStatNext.miniZeppelinStat.hitsToDie;
        }
        else if (_boughtInfo.type == MinionType.Healer.ToString())
        {
            curr = skillStat.skillHealAmount;
            next = skillStatNext.skillHealAmount;
        }
        else if (_boughtInfo.type == MinionType.WarScreamer.ToString())
        {
            curr = skillStat.activeSpeedDelta;
            next = skillStatNext.activeSpeedDelta;
        }

        item.SetItem(_boughtInfo.speed, curr, next, name);
        _list.Add(item);
    }

    void ReloadItems()
    {

    }

}
