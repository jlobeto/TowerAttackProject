using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildSquadManager : MonoBehaviour
{
    public List<SquadSelectedMinion> selectedButtons;
    public MinionInShop unselectedPrefab;
    public GridLayoutGroup scrollContent;

    GameManager _gm;
    User _user;
    List<MinionInShop> _totalMinionsList = new List<MinionInShop>();

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _user = _gm.User;
        FillScrollList();
        SetScrollButtons();
    }

    
    void Update()
    {
        
    }
    
    void FillScrollList()
    {
        var types = Enum.GetValues(typeof(MinionType)).Cast<MinionType>().Where(i => i != MinionType.MiniZeppelin).ToList();
        MinionInShop button;

        foreach (var item in types)
        {
            button = Instantiate(unselectedPrefab, scrollContent.transform);
            button.SetButton(item, "");
            _totalMinionsList.Add(button);
        }
    }

    void SetScrollButtons()
    {
        MinionBoughtDef bought;
        foreach (var item in _totalMinionsList)
        {
            bought = _user.GetMinionBought(item.minionType);
            item.IsBought = bought != null;
        }
    }
}
