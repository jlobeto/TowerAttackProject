using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildSquadManager : MonoBehaviour
{
    public ShopPopup shopPopup;
    public List<SquadSelectedMinion> selectedButtons;
    public MinionInShop unselectedPrefab;
    public GridLayoutGroup scrollContent;
    
    GameManager _gm;
    User _user;
    List<MinionInShop> _totalMinionsList = new List<MinionInShop>();
    Dictionary<MinionType, MinionStoreData> _storeInfoData;

    void Start()
    {

        _gm = FindObjectOfType<GameManager>();
        _user = _gm.User;
        _user.OnMinionBought += SetBoughtScrollButtons;

        var config = GameUtils.LoadConfig<GenericListJsonLoader<MinionStoreData>>
                        ("StoreMinionsData.json"
                        , GameUtils.MINION_CONFIG_PATH);

        _storeInfoData = new Dictionary<MinionType, MinionStoreData>();

        foreach (var item in config.list)
        {
            var minionType = (MinionType)Enum.Parse(typeof(MinionType), item.type);
            _storeInfoData.Add(minionType, item);
        }

        FillScrollList();
        SetBoughtScrollButtons();
    }

    
    void Update()
    {
        
    }
    
    void FillScrollList()
    {
        MinionInShop button;
        MinionType t;
        foreach (var item in _storeInfoData)
        {
            button = Instantiate(unselectedPrefab, scrollContent.transform);
            t = GameUtils.ToEnum(item.Value.type, MinionType.Runner);
            button.SetButton(t, "");
            button.onMinionClick += OnScrollButtonClicked;

            if (_user.LevelProgressManager.GetStarsAccumulated() < item.Value.starsNeedToUnlock)
                button.LockButton();

            _totalMinionsList.Add(button);
        }
    }

    void OnScrollButtonClicked(string desc, bool isBlocked, bool isBought, MinionType type)
    {
        if (isBlocked) return;

        if(!isBought)
        {
            shopPopup.DisplayPopup();
            shopPopup.SelectMinionByCode(type);
        }
    }

    void SetBoughtScrollButtons()
    {
        MinionBoughtDef bought;
        foreach (var item in _totalMinionsList)
        {
            bought = _user.GetMinionBought(item.minionType);
            item.IsBought = bought != null;
        }
    }
}
