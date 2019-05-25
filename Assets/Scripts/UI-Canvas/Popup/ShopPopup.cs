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

    protected override void Awake()
    {
        base.Awake();
        _popupManager = FindObjectOfType<PopupManager>();
        _gridGroup = GetComponentInChildren<GridLayoutGroup>();
        _shopManager = GetComponent<ShopManager>();
        _skillsUpgradePanel = GetComponentInChildren<SkillsUpgradePanel>();
        _skillsUpgradePanel.popupManager = _popupManager;
        _buyButtonText = buyButton.GetComponentInChildren<Text>();

        _scrollContentList = new List<MinionInShop>();
        gameObject.SetActive(false);
    }


    public override void DisplayPopup()
    {
        if (isShowing) return;

        _popupManager.ShopPopupDisplayed();

        _rect.position = new Vector3(_rect.parent.position.x - 12, _rect.parent.position.y);

        gameObject.SetActive(true);

        base.DisplayPopup();
    }

    public override void OkButtonPressed()
    {
        if (!isShowing) return;

        ExecuteFunctions(FunctionTypes.ok);
        _popupManager.DisplayedPopupWasClosed();
        gameObject.SetActive(false);
        isShowing = false;
    }

    public void AddMinionToShop(MinionType type, string description)
    {
        var m = Instantiate<MinionInShop>(minionInShopPrefab, _gridGroup.transform);
        m.SetButton(type, description);
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

    public void SelectMinion(MinionType type = MinionType.Runner)
    {
        var m = _scrollContentList.FirstOrDefault(i => i.minionType == type);
        if (m == null) return;

        m.button.onClick.Invoke();
    }

    void OnClickMinionButton(string info, bool isBlocked, bool isBought, MinionType type)
    {
        description.text = info;
        selected = type;

        var data = _shopManager.GetCurrencies(type);
        UpdateSkillsPanel();
        CheckBuyButton(isBlocked, isBought, data.Item1, data.Item2);
    }
    
}
