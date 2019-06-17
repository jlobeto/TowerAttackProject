﻿using System;
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

        FillSelectedList();
    }

    
    void Update()
    {
        
    }

    void FillSelectedList()
    {
        var selectedList = _user.GetSquadMinionsOrder();
        int index = 0;
        foreach (var item in selectedList)
        {
            selectedButtons[index].SetMinion(GameUtils.ToEnum(item, MinionType.Runner));
            selectedButtons[index].onMinionClick += OnSelectedMinionClickCallback;
            index++;
        }

        ChangeSelectedItemsColorInScroller(true);
    }

    void UnfillSelectedList()
    {
        foreach (var item in selectedButtons)
        {
            item.onMinionClick -= OnSelectedMinionClickCallback;
            item.ResetMinion();
        }
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
            return;
        }

        var orderList = _user.GetSquadMinionsOrder();

        if (orderList.Any(i => i == type.ToString()) || orderList.Count == selectedButtons.Count)
            return;

        _user.SetSquadMinionItem(type);

        var selectedItem = selectedButtons.FirstOrDefault(i => i.IsEmpty);
        selectedItem.SetMinion(type);
        selectedItem.onMinionClick += OnSelectedMinionClickCallback;

        var selectedItemScroll = _totalMinionsList.FirstOrDefault(i => i.minionType == type);
        if (selectedItemScroll != null)
            selectedItemScroll.ChangeToColor(true);
    }

    void ChangeSelectedItemsColorInScroller(bool value)
    {
        foreach (var item in selectedButtons)
        {
            if (item.IsEmpty) continue;

            var selectedItem = _totalMinionsList.FirstOrDefault(i => i.minionType == item.minionType);
            if (selectedItem != null)
                selectedItem.ChangeToColor(value);
        }
        
    }

    bool IsAlreadySelected(MinionType t )
    {
        return selectedButtons.Any(i => i.minionType == t);
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

    void OnSelectedMinionClickCallback(MinionType t)
    {
        var savedOrder = _user.GetSquadMinionsOrder();
        var s = t.ToString();

        if (savedOrder.Any( i => i == s))
        {
            _user.DeleteSquadMinionItem(t);
            var selectedItem = _totalMinionsList.FirstOrDefault(i => i.minionType == t);
            selectedItem.ChangeToColor(false);
            UnfillSelectedList();
            FillSelectedList();
        }
    }

}
