using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionInShop : MonoBehaviour
{

    public Button button;
    public Action<string, bool, bool, MinionType> onMinionClick = delegate { };
    public MinionType minionType;

    bool _isBought;
    bool _isBlocked;
    Text _buttonText;
    string _description;

    public bool IsBought { get { return _isBought; } set { _isBought = value; } }

    void Awake()
    {
        _buttonText = button.GetComponentInChildren<Text>();
        button.onClick.AddListener(MinionInShopClick);
    }


    void Update()
    {

    }

    public void SetButton(MinionType t, string desc)
    {
        _buttonText.text = t.ToString();
        minionType = t;
        _description = desc;
    }
    

    /// <summary>
    /// This function will block the button so the player can't buy the minion.
    /// Also it will change the UI to seems like it is blocked.
    /// </summary>
    public void LockButton()
    {
        if (_isBlocked) return;
        
        _isBlocked = true;
    }

    public void UnlockButton()
    {
        if (!_isBlocked) return;

        _isBlocked = false;
    }


    void MinionInShopClick()
    {
        onMinionClick(_description, _isBlocked, IsBought, minionType);
    }
}
