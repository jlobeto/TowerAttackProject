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
    public Action OnPlayPressed = delegate { };
    
    GameManager _gm;
    User _user;
    List<MinionInShop> _totalMinionsList = new List<MinionInShop>();
    Dictionary<MinionType, MinionStoreData> _storeInfoData;
    PopupManager _popupManager;

    void Awake()
    {
        _gm = FindObjectOfType<GameManager>();
        _popupManager = FindObjectOfType<PopupManager>();
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

    public void OnPlay()
    {
        if (_user.GetSquadMinionsOrder().Count == 0)
        {
            foreach (var item in selectedButtons)
            {
                item.DoRedColorAnimation();
            }
            return;
        }

        OnPlayPressed();
        OnExit();
    }

    public void OnExit()
    {
        gameObject.SetActive(false);
        _popupManager.DisplayedPopupWasClosed();
    }

    public void DisplayPopup()
    {
        gameObject.SetActive(true);
        _popupManager.PopupDisplayed();

        SetBoughtScrollButtons();
        FillSelectedList();
        UpdateScrollerPadlocks();
    }

    void FillSelectedList()
    {
        var selectedList = _user.GetSquadMinionsOrder();
        int index = 0;
        foreach (var item in selectedList)
        {
            selectedButtons[index].SetMinion(GameUtils.ToEnum(item, MinionType.Runner), _gm);
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


    /// <summary>
    /// this has to be called once, at the init of the world selector scene. (first time the popup is shown, at the awake())
    /// </summary>
    void FillScrollList()
    {
        MinionInShop button;
        MinionType t;
        var i = 0;
        foreach (var item in _storeInfoData)
        {
            button = Instantiate(unselectedPrefab, scrollContent.transform);
            t = GameUtils.ToEnum(item.Value.type, MinionType.Runner);
            button.SetButton(t, "", _gm);
            button.onMinionClick += OnScrollButtonClicked;
            button.name = "build_squad_unselected_" + i;
            CheckScrollItemPadlock(item.Value, button);

            _totalMinionsList.Add(button);
            i++;
        }
    }

    void UpdateScrollerPadlocks()
    {
        MinionStoreData storeDataDef;
        foreach (var item in _totalMinionsList)
        {
            storeDataDef = _storeInfoData[item.minionType];
            CheckScrollItemPadlock(storeDataDef, item);
        }
    }

    void CheckScrollItemPadlock(MinionStoreData storeDataDef, MinionInShop button)
    {
        if (_user.LevelProgressManager.GetStarsAccumulated() < storeDataDef.starsNeedToUnlock)
            button.LockButton();
        else
            button.UnlockButton();
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
        selectedItem.SetMinion(type, _gm);
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
        ChangeSelectedItemsColorInScroller(true);
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
