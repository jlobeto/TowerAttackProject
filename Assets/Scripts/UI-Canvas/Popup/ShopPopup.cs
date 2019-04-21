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
    public Button buyButton;
    public MinionType selected;
    public PopupManager popupManager;

    SkillsUpgradePanel _skillsUpgradePanel;
    GridLayoutGroup _gridGroup;
    List<MinionInShop> _scrollContentList;
    ShopManager _shopManager;
    Text _buyButtonText;

    protected override void Awake()
    {
        base.Awake();
        _gridGroup = GetComponentInChildren<GridLayoutGroup>();
        _scrollContentList = new List<MinionInShop>();
        _shopManager = GetComponent<ShopManager>();
        _skillsUpgradePanel = GetComponentInChildren<SkillsUpgradePanel>();
        _skillsUpgradePanel.popupManager = popupManager;
        _buyButtonText = buyButton.GetComponentInChildren<Text>();
    }


    public override void DisplayPopup()
    {
        if (isShowing) return;

        _rect.position = new Vector3(_rect.parent.position.x - 12, _rect.parent.position.y);

        gameObject.SetActive(true);
        base.DisplayPopup();
    }

    public override void OkButtonPressed()
    {
        if (!isShowing) return;

        ExecuteFunctions(FunctionTypes.ok);
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

    void OnClickMinionButton(string info, bool isBlocked, bool isBought, MinionType type)
    {
        description.text = info;
        selected = type;

        var data = _shopManager.GetMinionShopInfo(type);
        if (data.Item1 != null)
        {
            _skillsUpgradePanel.SetUpgradeItems(data.Item1, data.Item2, data.Item3);
            CheckBuyButton(isBlocked, isBought, data.Item4, data.Item5);
        }
        else
        {
            _skillsUpgradePanel.HideAllStats();
            CheckBuyButton(isBlocked, isBought, data.Item4, data.Item5);
        }

    }

}
