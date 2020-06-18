using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopPopup : BasePopup
{
    public MinionInShop minionInShopPrefab;
    public Text currency;
    public Text starsText;
    public Button buyButton;
    public MinionType selected;

    PopupManager _popupManager;
    SkillsUpgradePanel _skillsUpgradePanel;
    GridLayoutGroup _gridGroup;
    List<MinionInShop> _scrollContentList;
    ShopManager _shopManager;
    Text _buyButtonText;
    ScrollRect _scrollRect;
    Canvas _thisCanvas;
    bool _goingToItemInScroller;
    float _showItemInScrollerResult;

    protected override void Awake()
    {
        base.Awake();
        _popupManager = FindObjectOfType<PopupManager>();
        _gridGroup = GetComponentInChildren<GridLayoutGroup>();
        _scrollRect = GetComponentInChildren<ScrollRect>();
        _shopManager = GetComponent<ShopManager>();
        _skillsUpgradePanel = GetComponentInChildren<SkillsUpgradePanel>();
        _skillsUpgradePanel.popupManager = _popupManager;
        _buyButtonText = buyButton.GetComponentInChildren<Text>();

        _scrollContentList = new List<MinionInShop>();
        _thisCanvas = GetComponent<Canvas>();
        _thisCanvas.enabled = false;
    }
    

    private void Update()
    {
        //To move the selected item to the middle of the scroller
        if(_goingToItemInScroller)
        {
            _scrollRect.horizontalNormalizedPosition = Mathf.Lerp(_scrollRect.horizontalNormalizedPosition, _showItemInScrollerResult, Time.deltaTime * 8);
            if (Mathf.Abs(_scrollRect.horizontalNormalizedPosition  - _showItemInScrollerResult) < 0.005f)
                _goingToItemInScroller = false;
        }

    }


    public override void DisplayPopup()
    {
        if (isShowing) return;

        _popupManager.PopupDisplayed();

        _rect.position = new Vector3(_rect.parent.position.x-3, _rect.parent.position.y);

        _thisCanvas.enabled = true;

        base.DisplayPopup();
    }

    public override void OkButtonPressed()
    {
        if (!isShowing) return;

        ExecuteFunctions(FunctionTypes.ok);
        _popupManager.DisplayedPopupWasClosed();
        _thisCanvas.enabled = false;
        isShowing = false;
    }

    public void AddMinionToShop(MinionType type, string description, int index)
    {
        var m = Instantiate<MinionInShop>(minionInShopPrefab, _gridGroup.transform);
        m.SetButton(type, description);
        m.name = "MinionInShop_" + index;
        m.onMinionClick += OnClickMinionButton;
        _scrollContentList.Add(m);
    }

    public void CheckMinionAvailability(MinionType type, bool isBlocked, bool isBought)
    {
        var minionBtn = _scrollContentList.FirstOrDefault(i => i.minionType == type);
        if (!minionBtn) return;

        if (isBlocked)
            minionBtn.LockButton();
        else
        {
            minionBtn.UnlockButton();
            minionBtn.IsBought = isBought;
        }
    }

    public void SetCurrency(int c)
    {
        currency.text = "CHIPS: " + c;
    }

    public void SetStars(int s)
    {
        starsText.text = "STARS: " + s;
    }

    public void CheckBuyButton(bool isBlocked, bool isBought, int starsToUnlock = 0, int price = 0)
    {
        if (isBlocked)
        {
            buyButton.gameObject.SetActive(true);
            buyButton.interactable = false;
            _buyButtonText.text = "NEED " + starsToUnlock.ToString() + " STARS";
        }
        else
        {
            buyButton.interactable = true;
            buyButton.gameObject.SetActive(!isBought);

            if(!isBought)
                _buyButtonText.text = "BUY FOR " + price + " CHIPS";
        }
    }

    public void UpdateSkillsPanel()
    {
        var data = _shopManager.GetMinionShopInfo(selected);
        if (data.Item1 != null)
        {
            _skillsUpgradePanel.SetUpgradeItems(data.Item1, data.Item2, data.Item3);
        }
        else
        {
            _skillsUpgradePanel.HideAllStats();
        }
    }

    public void SelectMinionByCode(MinionType type = MinionType.Runner)
    {
        MinionInShop m = null;
        
        float i = 0;
        foreach (var item in _scrollContentList)
        {
            if (item.minionType == type)
            {
                m = item;
                break;
            }

            i += 1;
        }

        if (m == null) return;

        m.button.onClick.Invoke();
        ShowItemInScroller(i);
    }

    void OnClickMinionButton(string info, bool isBlocked, bool isBought, MinionType newSelectedType)
    {
        var newSelected = _scrollContentList.FirstOrDefault(i => i.minionType == newSelectedType);
        var lastSelected = _scrollContentList.FirstOrDefault(i => i.minionType == selected);
        newSelected.ChangeToColor(true);

        if (selected != newSelectedType)
            lastSelected.ChangeToColor(false);

        description.text = info;
        selected = newSelectedType;

        var data = _shopManager.GetCurrencies(newSelectedType);
        UpdateSkillsPanel();
        CheckBuyButton(isBlocked, isBought, data.Item1, data.Item2);
    }

    void ShowItemInScroller(float i)
    {
        float scrollNewValue = 0;
        if (i == _scrollContentList.Count - 1)
            scrollNewValue = 1;
        else if (i != 0)
        {
            scrollNewValue = i / _scrollContentList.Count;
            if (scrollNewValue > 0.5f)
                scrollNewValue += (1.0f / 6);
            else if (scrollNewValue < 0.4f)
                scrollNewValue -= (1.0f / 6);

            if (scrollNewValue > 1.0f) scrollNewValue = 1f;
            if (scrollNewValue < 0.0f) scrollNewValue = 0f;

        }

        _showItemInScrollerResult = scrollNewValue;
        _goingToItemInScroller = true;
    }


}
