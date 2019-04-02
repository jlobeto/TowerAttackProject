using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : BasePopup
{
    public MinionInShop minionInShopPrefab;
    GridLayoutGroup _gridGroup;

    protected override void Awake()
    {
        base.Awake();
        _gridGroup = GetComponentInChildren<GridLayoutGroup>();
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
        m.SetButton(type.ToString());
        m.SetDescription(description);
        m.onMinionClick += ChangeMinionInfo;
    }

    void ChangeMinionInfo(string info)
    {
        description.text = info;
    }

}
