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
    SoundManager _soundManager;



    void Start()
    {
        _popManager = FindObjectOfType<PopupManager>();
        _shopManager = GetComponentInParent<ShopManager>();
        _soundManager = FindObjectOfType<SoundManager>();
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

    void StatBuyPressed(MinionBoughtDef.StatNames  id, StatUpgradeItem item)
    {
        var couldBuy = _shopManager.BuyStat(id);
        if (!couldBuy)
        {
            _soundManager.PlaySound(SoundFxNames.fail_buy);
            item.NoCoinsAnimation();
        }
        else
            _soundManager.PlaySound(SoundFxNames.upgrade_success);
            
    }
    

    void GenerateItemsFromScratch()
    {
        var currAndNext = GetCurrentAndNextStat(_boughtInfo.hp);

        var item = Instantiate<StatUpgradeItem>(itemGO, transform); 
        var name = GetFirstStatName();
        item.OnBuyClick += StatBuyPressed;

        float curr = currAndNext.Item1.hp;
        float next = currAndNext.Item2.hp;

        if (_boughtInfo.type == MinionType.Healer.ToString() /*|| _boughtInfo.type == MinionType.WarScreamer.ToString() WARSCREAMER DOES NOT HAVE PASSIVE ANYMORE */)
        {
            currAndNext = GetCurrentAndNextStat(_boughtInfo.passiveSkill);

            curr = currAndNext.Item1.GetStatByStatId(MinionBoughtDef.StatNames.PASSIVE, GameUtils.ToEnum(_boughtInfo.type, MinionType.Runner));
            next = currAndNext.Item2.GetStatByStatId(MinionBoughtDef.StatNames.PASSIVE, GameUtils.ToEnum(_boughtInfo.type, MinionType.Runner));

            item.SetItem(_boughtInfo.passiveSkill, curr, next, name, _statsCurrency.GetPrice(name, _boughtInfo.GetStatLevel(name) + 1));
        }
        else
            item.SetItem(_boughtInfo.hp, curr, next, name, _statsCurrency.GetPrice(name, _boughtInfo.GetStatLevel(name) + 1));

        _list = new List<StatUpgradeItem>();
        _list.Add(item);

        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        item.OnBuyClick += StatBuyPressed;
        name = MinionBoughtDef.StatNames.SPD;
        currAndNext = GetCurrentAndNextStat(_boughtInfo.speed);

        curr = currAndNext.Item1.speed;
        next = currAndNext.Item2.speed;
        item.SetItem(_boughtInfo.speed, curr, next, name, _statsCurrency.GetPrice(name, _boughtInfo.GetStatLevel(name) + 1));
        _list.Add(item);
        
        item = Instantiate<StatUpgradeItem>(itemGO, transform);
        item.OnBuyClick += StatBuyPressed;
        name = MinionBoughtDef.StatNames.SKILL;
        
        currAndNext = GetCurrentAndNextStat(_boughtInfo.skill);

        curr = currAndNext.Item1.GetStatByStatId(MinionBoughtDef.StatNames.SKILL, GameUtils.ToEnum(_boughtInfo.type, MinionType.Runner));
        next = currAndNext.Item2.GetStatByStatId(MinionBoughtDef.StatNames.SKILL, GameUtils.ToEnum(_boughtInfo.type, MinionType.Runner));

        item.SetItem(_boughtInfo.skill, curr, next, name, _statsCurrency.GetPrice(name, _boughtInfo.GetStatLevel(name) + 1));
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
        return _statsCurrency.hpPerLevel != null ? MinionBoughtDef.StatNames.HP : MinionBoughtDef.StatNames.PASSIVE;
    }

    Tuple<BaseMinionStat, BaseMinionStat> GetCurrentAndNextStat(int lvl)
    {
        var curr = _minionStats.list.FirstOrDefault(i => i.levelId == lvl);
        var next = _minionStats.list.FirstOrDefault(i => i.levelId == lvl + 1);
        if (next == null)
            next = curr;
        return Tuple.Create(curr, next);
    }

    

}
