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

    SkillsUpgradePanel _skillsUpgradePanel;
    GridLayoutGroup _gridGroup;
    List<MinionInShop> _scrollContentList;
    ShopManager _shopManager;

    protected override void Awake()
    {
        base.Awake();
        _gridGroup = GetComponentInChildren<GridLayoutGroup>();
        _scrollContentList = new List<MinionInShop>();
        _shopManager = GetComponent<ShopManager>();
        _skillsUpgradePanel = GetComponentInChildren<SkillsUpgradePanel>();
    }


    public override void DisplayPopup()
    {
        if (isShowing) return;

        _rect.position = new Vector3(_rect.parent.position.x - 10, _rect.parent.position.y);

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

    public void CheckBuyButton(bool isBlocked, bool isBought)
    {
        if (isBlocked)
        {
            buyButton.gameObject.SetActive(true);
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
            buyButton.gameObject.SetActive(!isBought);
        }
    }

    void OnClickMinionButton(string info, bool isBlocked, bool isBought, MinionType type)
    {
        description.text = info;
        selected = type;
        CheckBuyButton(isBlocked, isBought);
        var data = _shopManager.OnMinionShopClick(type);
        if (data != null)
            _skillsUpgradePanel.SetUpgradeItems(data.Item1, data.Item2, data.Item3);
        else
            _skillsUpgradePanel.HideAllStats();
    }

}
