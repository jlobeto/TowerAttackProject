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
        var config = GameUtils.LoadConfig<GenericListJsonLoader<MinionStoreData>>
                        ("StoreMinionsData.json"
                        , GameUtils.MINION_CONFIG_PATH);

        MinionType minionType;
        foreach (var item in config.list)
        {
            minionType = (MinionType)Enum.Parse(typeof(MinionType), item.type);
            _storeInfoData.Add(minionType, item);
            _popup.AddMinionToShop(minionType, CreateDescriptionString(item));
        }

        _storeStatsCurrencyDef = new Dictionary<MinionType, MinionsStatsCurrencyDef>();
        var config2 = GameUtils.LoadConfig<GenericListJsonLoader<MinionsStatsCurrencyDef>>("StoreMinionsStatsUpgrade.json", GameUtils.MINION_CONFIG_PATH);

        foreach (var item2 in config2.list)
        {
            minionType = (MinionType)Enum.Parse(typeof(MinionType), item2.type);
            _storeStatsCurrencyDef.Add(minionType, item2);
        }
    }

    
    void Update()
    {
        
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
    }
}
