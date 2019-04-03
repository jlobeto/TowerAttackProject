using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    Dictionary<MinionType, MinionStoreInfo> _storeInfo;
    ShopPopup _popup;
    GameManager _gm;
    char _descriptionBullet = '\u25A0';

    void Start()
    {
        _popup = GetComponent<ShopPopup>();
        _popup.AddFunction(BasePopup.FunctionTypes.displayCallback, OnPopupDisplayCallback);

        _gm = FindObjectOfType<GameManager>();

        _storeInfo = new Dictionary<MinionType, MinionStoreInfo>();
        var config = GameUtils.LoadConfig<GenericListJsonLoader<MinionStoreInfo>>
                        ("StoreMinionsInfo.json"
                        , GameUtils.MINION_CONFIG_PATH);

        foreach (var item in config.list)
        {
            MinionType type = (MinionType)Enum.Parse(typeof(MinionType), item.type);
            _storeInfo.Add(type, item);
            _popup.AddMinionToShop(type, CreateDescriptionString(item));
        }
    }

    
    void Update()
    {
        
    }

    string CreateDescriptionString(MinionStoreInfo info)
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
        _popup.SetCurrency(_gm.User.Currency);
    }
}
