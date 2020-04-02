using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public const int MAX_MINION_LEVEL = 15;
    public const string SPEED_STAT_DESC = "Increment your minion's speed!";
    public const string HP_STAT_DESC = "More health means easy match!";

    Dictionary<MinionType, MinionStoreData> _storeInfoData;
    Dictionary<MinionType, MinionsStatsCurrencyDef> _storeStatsCurrencyDef;
    ShopPopup _popup;
    GameManager _gm;
    char _descriptionBullet = '\u25A0';

    void Start()
    {
        _popup = GetComponent<ShopPopup>();
        _popup.AddFunction(BasePopup.FunctionTypes.displayCallback, OnPopupDisplayCallback);

        _gm = FindObjectOfType<GameManager>();

        _storeInfoData = new Dictionary<MinionType, MinionStoreData>();
        _storeStatsCurrencyDef = new Dictionary<MinionType, MinionsStatsCurrencyDef>();
        var config = GameUtils.LoadConfig<GenericListJsonLoader<MinionStoreData>>
                        ("StoreMinionsData.json"
                        , GameUtils.MINION_CONFIG_PATH);

        var config2 = GameUtils.LoadConfig<GenericListJsonLoader<MinionsStatsCurrencyDef>>
                        ("StoreMinionsStatsUpgrade.json"
                        , GameUtils.MINION_CONFIG_PATH);


        MinionType minionType;

        foreach (var item2 in config2.list)
        {
            minionType = (MinionType)Enum.Parse(typeof(MinionType), item2.type);
            _storeStatsCurrencyDef.Add(minionType, item2);
        }

        foreach (var item in config.list)
        {
            minionType = (MinionType)Enum.Parse(typeof(MinionType), item.type);
            _storeInfoData.Add(minionType, item);
            _popup.AddMinionToShop(minionType, CreateDescriptionString(item));
        }

        OnPopupDisplayCallback();
    }

    
    void Update()
    {
        
    }

    public void BuyMinion()
    {
        if (_gm.User.MinionIsInInvetory(_popup.selected))
            return;

        var currency = _gm.User.Currency;
        var minionValue = _storeInfoData[_popup.selected].currencyValue;

        var dif = currency - minionValue;

        if (dif < 0)
        {
            Debug.Log("You don't have currency for this buy.");
            return;
        }

        _gm.User.BuyMinion(_popup.selected, minionValue);

        //Call again to refresh data.
        UpdatePopupVisualData(true);
        _popup.CheckBuyButton(false, true);
        _popup.SelectMinionByCode(_popup.selected);
    }

    public bool BuyStat(MinionBoughtDef.StatNames  id)
    {
        var currency = _gm.User.Currency;
        var currLevel = _gm.User.GetMinionBought(_popup.selected).GetStatLevel(id);
        var statPrice = _storeStatsCurrencyDef[_popup.selected].GetPrice(id, currLevel+1);
        if (currency - statPrice < 0)
            return false;

        _gm.User.BuyMinionStat(_popup.selected, id, Mathf.CeilToInt( statPrice));
        //Call again to refresh data.
        _popup.SetCurrency(_gm.User.Currency);
        _popup.UpdateSkillsPanel();

        return true;
    }

    public Tuple<MinionBoughtDef, MinionsStatsCurrencyDef, GenericListJsonLoader<BaseMinionStat>> 
        GetMinionShopInfo(MinionType t)
    {
        var boughtInfo = _gm.User.GetMinionBought(t);

        if (boughtInfo == null)
        {
            return Tuple.Create(boughtInfo
            , default(MinionsStatsCurrencyDef)
            , default(GenericListJsonLoader<BaseMinionStat>));
        }

        var statsCurr = _storeStatsCurrencyDef[t];
        var minionStats = _gm.MinionsJsonLoader.GetMinionStats(t);

        return Tuple.Create(boughtInfo, statsCurr, minionStats);
    }

    public Tuple<int,int> GetCurrencies(MinionType t)
    {
        var needToUnlockBuy = _storeInfoData[t].starsNeedToUnlock - GetUserTotalStars();
        var price = _storeInfoData[t].currencyValue;
        return Tuple.Create(needToUnlockBuy, price);
    }

    public string GetMinionUpgradeDescription(string minionType, MinionBoughtDef.StatNames statId)
    {
        MinionType type = GameUtils.ToEnum(minionType, MinionType.Runner);
        var list = new List<string>();
        var storeData = _storeInfoData[type];
        var statInfo = storeData.statsInfo.FirstOrDefault(i => i.type == statId.ToString());
        if (statInfo != null)
            return statInfo.info;

        return ""; 
    }

    string CreateDescriptionString(MinionStoreData info)
    {
        var result = "";
        string bullet = _descriptionBullet.ToString();
        foreach (var item in info.info)
        {
            result += bullet + "  " + item + "\n";
        }

        return result;
    }

    void OnPopupDisplayCallback(string p = "")
    {
        UpdatePopupVisualData();
    }

    void UpdatePopupVisualData(bool boughtUpdate = false)
    {
        _popup.SetCurrency(_gm.User.Currency);
        _popup.SetStars(GetUserTotalStars());

        foreach (var item in _storeInfoData)
        {
            _popup.CheckMinionAvailability(item.Key, GetUserTotalStars() < item.Value.starsNeedToUnlock, IsMinionBought(item.Key));
        }

        if (!boughtUpdate)
            _popup.SelectMinionByCode();//this goes after the check minion availability due the the selection feedback on runner.
    }

    int GetUserTotalStars()
    {
        return _gm.User.LevelProgressManager.GetStarsAccumulated();
    }

    bool IsMinionBought(MinionType type)
    {
        return _gm.User.MinionIsInInvetory(type);
    }
}
