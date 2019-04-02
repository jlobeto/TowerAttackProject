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

    public void SetButton(string text)
    {
        _buttonText.text = text;
    }

    public void SetDescription(string desc)
    {
        _description = desc;
    }

    void MinionInShopClick()
    {
        onMinionClick(_description);
    }
}
