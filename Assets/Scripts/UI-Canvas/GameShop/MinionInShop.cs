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
    public Color unableToPurchaseColor;

    bool _isBought;
    bool _isBlocked;
    Text _buttonText;
    string _description;
    ColorBlock _colorBlock;


    public bool IsBought { get { return _isBought; } set { _isBought = value; } }

    void Awake()
    {
        _buttonText = button.GetComponentInChildren<Text>();
        button.onClick.AddListener(MinionInShopClick);
        _colorBlock = button.colors;
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
        _colorBlock.normalColor = unableToPurchaseColor;
        _colorBlock.highlightedColor = unableToPurchaseColor;
        button.colors = _colorBlock;
    }

    public void UnlockButton()
    {
        if (!_isBlocked) return;

        _isBlocked = false;
        _colorBlock.normalColor = Color.white;
        _colorBlock.highlightedColor = Color.white;
        button.colors = _colorBlock;
    }


    void MinionInShopClick()
    {
        onMinionClick(_description, _isBlocked, IsBought, minionType);
    }
}
