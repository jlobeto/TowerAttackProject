using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

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

        _popup.onMinionClick += OnMinionShopClick;
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
        OnPopupDisplayCallback();
        _popup.CheckBuyButton(false, true);
    }

    void OnMinionShopClick(MinionType t)
    {
        var boughtInfo = _gm.User.GetMinionBought(t);
        if (boughtInfo == null)
        {
            _popup.skillsUpgradePanel.HideAllStats();
            return;
        }

        var statsCurr = _storeStatsCurrencyDef[t];
        var minionStats = _gm.MinionsJsonLoader.GetMinionStats(t);

        _popup.skillsUpgradePanel.SetUpgradeItems(boughtInfo, statsCurr, minionStats);
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

    void OnPopupDisplayCallback()
    {
        //TODO :: Setear tambien al Runner como seleccionado default (asi muestra la info de este).
        _popup.SetCurrency(_gm.User.Currency);
        foreach (var item in _storeInfoData)
        {
            _popup.CheckMinionAvailability(item.Key, GetUserTotalStars() < item.Value.starsNeedToUnlock, IsMinionBought(item.Key));
        }
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
