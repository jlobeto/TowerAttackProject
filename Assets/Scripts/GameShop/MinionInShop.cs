using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionInShop : MonoBehaviour
{

    public Button button;
    public Button buyButton;
    public Action<string> onMinionClick = delegate { };
    public MinionType minionType;

    bool _isBlocked;
    Text _buttonText;
    string _description;

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

        buyButton.interactable = false;
        _isBlocked = true;
    }

    public void UnlockButton()
    {
        if (!_isBlocked) return;

        buyButton.interactable = true;
        _isBlocked = false;
    }


    void MinionInShopClick()
    {
        onMinionClick(_description);
    }
}
