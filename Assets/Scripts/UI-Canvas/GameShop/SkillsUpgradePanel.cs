using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SkillsUpgradePanel : MonoBehaviour
{

    public StatUpgradeItem itemGO;
    [HideInInspector]public PopupManager popupManager;

    List<StatUpgradeItem> _list;
    MinionBoughtDef _boughtInfo;
    MinionsStatsCurrencyDef _statsCurrency;
    GenericListJsonLoader<BaseMinionStat> _minionStats;
    PopupManager _popManager;
    ShopManager _shopManager;



    void Start()
    {
        _popManager = FindObjectOfType<PopupManager>();
        _shopManager = GetComponentInParent<ShopManager>();
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

    void StatBuyPressed(MinionBoughtDef.StatNames  id)
    {
        var statLvl = _boughtInfo.GetStatLevel(id);
        var desc = _shopManager.GetMinionUpgradeDescription(_boughtInfo.type, id);
        var btnText = _statsCurrency.GetPrice(id, statLvl) + " CHIPS";
        var values = GetCurrentAndNextStat(statLvl);
        var popup = _popManager.BuildPopup(transform.parent, _boughtInfo.type , desc, btnText, PopupsID.StatUpgradeShop) as StatUpgradePopup;
        var minionType = GameUtils.ToEnum(_boughtInfo.type, MinionType.Runner);
        popup.SetPopup(id, values.Item1.GetStatByStatId(id, minionType).ToString(), values.Item2.GetStatByStatId(id, minionType).ToString(), statLvl);

    }

    void GenerateItemsFromScratch()
    {
        ////TODO>>>> Eliminar los ifs por lo que esta en BaseMinionStat
        var currAndNext = GetCurrentAndNextStat(_boughtInfo.hp);

        var item = Instantiate<StatUpgradeItem>(itemGO, transform); 
        var name = GetFirstStatName();
        item.OnBuyClick += StatBuyPressed;

        float curr = currAndNext.Item1.hp;
        float next = currAndNext.Item2.hp;

        if (_boughtInfo.type == MinionType.Healer.ToString())
        {
            currAndNext = GetCurrentAndNextStat(_boughtInfo.passiveSkill);
            curr = currAndNext.Item1.healPerSecond;
            next = currAndNext.Item2.healPerSecond;
        }
        else if(_boughtInfo.type == MinionType.WarScreamer.ToString())
        {
            currAndNext = GetCurrentAndNextStat(_boughtInfo.passiveSkill);
            curr = currAndNext.Item1.passiveSpeedDelta;
            next = currAndNext.Item2.passiveSpeedDelta;
        }
        
        item.SetItem(_boughtInfo.hp, curr, next, name);

        _list = new List<StatUpgradeItem>();
        _list.Add(item);

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        item.OnBuyClick += StatBuyPressed;
        name = MinionBoughtDef.StatNames.SPD;
        currAndNext = GetCurrentAndNextStat(_boughtInfo.speed);

        curr = currAndNext.Item1.speed;
        next = currAndNext.Item2.speed;
        item.SetItem(_boughtInfo.speed, curr, next, name);
        _list.Add(item);

        if (_boughtInfo.type == MinionType.Dove.ToString())
            return;

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        item.OnBuyClick += StatBuyPressed;
        name = MinionBoughtDef.StatNames.SKILL;
        
        currAndNext = GetCurrentAndNextStat(_boughtInfo.skill);

        if (_boughtInfo.type == MinionType.Runner.ToString())
        {
            curr = currAndNext.Item1.skillDeltaSpeed;
            next = currAndNext.Item2.skillDeltaSpeed;
        }
        else if (_boughtInfo.type == MinionType.Tank.ToString())
        {
            curr = currAndNext.Item1.skillArea;
            next = currAndNext.Item2.skillArea;
        }
        else if (_boughtInfo.type == MinionType.Zeppelin.ToString())
        {
            curr = currAndNext.Item1.miniZeppelinStat.hitsToDie;
            next = currAndNext.Item2.miniZeppelinStat.hitsToDie;
        }
        else if (_boughtInfo.type == MinionType.Healer.ToString())
        {
            curr = currAndNext.Item1.skillHealAmount;
            next = currAndNext.Item2.skillHealAmount;
        }
        else if (_boughtInfo.type == MinionType.WarScreamer.ToString())
        {
            curr = currAndNext.Item1.activeSpeedDelta;
            next = currAndNext.Item2.activeSpeedDelta;
        }

        item.SetItem(_boughtInfo.skill, curr, next, name);
        _list.Add(item);
    }

    void ReloadItems()
    {
        foreach (var item in _list)
        {
            item.OnBuyClick -= StatBuyPressed;
            Destroy(item.gameObject);
        }

        ///THIS MUST CHANGE SO IT DOES NOT CREATE AN INSTANCE EACH TIME
        GenerateItemsFromScratch();
            
    }


    MinionBoughtDef.StatNames GetFirstStatName()
    {
        return _statsCurrency.baseHPCurrencyValue > 0 ? MinionBoughtDef.StatNames.HP : MinionBoughtDef.StatNames.PASSIVE;
    }

    Tuple<BaseMinionStat, BaseMinionStat> GetCurrentAndNextStat(int lvl)
    {
        var curr = _minionStats.list.FirstOrDefault(i => i.levelId == lvl);
        var next = _minionStats.list.FirstOrDefault(i => i.levelId == lvl + 1);
        return Tuple.Create(curr, next);
    }

    

}
