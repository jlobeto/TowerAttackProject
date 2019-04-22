using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This will contain all the info that have to be shown to the user about the stat upgrade.
/// </summary>
public class StatUpgradeItem : MonoBehaviour
{
    public Button buyButton;
    public Text statName;
    public Text currValue;
    public Text nextValue;
    public Image levelBar;
    public MinionBoughtDef.StatNames id;
    public Action<MinionBoughtDef.StatNames> OnBuyClick = delegate { };

    MinionsStatsCurrencyDef _statsCurrencyDef = new MinionsStatsCurrencyDef();

    void Start()
    {

    }
    
    void Update()
    {
        
    }

    public void SetItem(int statLvl, float currValue, float nextValue, MinionBoughtDef.StatNames id)
    {
        this.id = id;
        statName.text = id.ToString() + " :";
        this.currValue.text = currValue.ToString();
        this.nextValue.text = nextValue.ToString();

        levelBar.fillAmount = float.Parse(statLvl.ToString()) / ShopManager.MAX_MINION_LEVEL;

        buyButton.onClick.AddListener(() => BuyPressed());
    }

    void BuyPressed()
    {
        OnBuyClick(id);
    }

    bool IsDove()
    {
        return _statsCurrencyDef.type == MinionType.Dove.ToString();
    }
}
