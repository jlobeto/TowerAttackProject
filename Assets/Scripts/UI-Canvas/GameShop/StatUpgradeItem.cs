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

    const int MAX_LEVEL = 15;

    MinionsStatsCurrencyDef _statsCurrencyDef;

    void Start()
    {
    }
    
    void Update()
    {
        
    }

    public void SetItem(int statLvl, float currValue, float nextValue, string name)
    {
        statName.text = name;
        this.currValue.text = currValue.ToString();
        this.nextValue.text = nextValue.ToString();

        levelBar.fillAmount = float.Parse(statLvl.ToString()) / MAX_LEVEL;
    }

    bool IsDove()
    {
        return _statsCurrencyDef.type == MinionType.Dove.ToString();
    }
}
