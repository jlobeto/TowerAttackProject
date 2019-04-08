using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : BasePopup
{
    public MinionInShop minionInShopPrefab;
    public Text currency;

    GridLayoutGroup _gridGroup;
    List<MinionInShop> _scrollContentList;

    protected override void Awake()
    {
        base.Awake();
        _gridGroup = GetComponentInChildren<GridLayoutGroup>();
        _scrollContentList = new List<MinionInShop>();
    }


    public override void DisplayPopup()
    {
        if (isShowing) return;

        _rect.position = new Vector3(_rect.parent.position.x, _rect.parent.position.y);
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
        m.onMinionClick += ChangeMinionInfo;
        _scrollContentList.Add(m);
    }

    public void CheckMinionAvailability(MinionType type, bool isBlocked)
    {
        var minionBtn = _scrollContentList.FirstOrDefault(i => i.minionType == type);
        if (!minionBtn) return;

        if (isBlocked)
            minionBtn.LockButton();
        else
            minionBtn.UnlockButton();
    }

    public void SetCurrency(int c)
    {
        currency.text = "CHIPS: " + c; 
    }

    void ChangeMinionInfo(string info)
    {
        description.text = info;
    }

}
