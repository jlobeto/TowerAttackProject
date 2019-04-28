using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinionInShop : MonoBehaviour
{

    public Button button;
    public Image minionPic;
    public Image padLock;
    public Action<string, bool, bool, MinionType> onMinionClick = delegate { };
    public MinionType minionType;
    public Color unableToPurchaseColor;
    public Color nameTextColor;

    bool _isBought;
    bool _isBlocked;
    Text _buttonText;
    string _description;
    ColorBlock _colorBlock;
    Color _lastColor;


    public bool IsBought
    {
        get { return _isBought; }
        set
        {
            _isBought = value;

            if (_isBought)
                SetBoughtColors();
            else
                SetNotBoughtColors();
        }
    }

    void Awake()
    {
        _buttonText = button.GetComponentInChildren<Text>();
        button.onClick.AddListener(MinionInShopClick);
        _colorBlock = button.colors;
        var pointer = button.GetComponent<OnCustomPointerCallback>();
        pointer.AddListener(OnCustomPointerCallback.Listener.pointerDown, OnPointerDown);
        pointer.AddListener(OnCustomPointerCallback.Listener.pointerUp, OnPointerUp);

        padLock.enabled = false;
    }


    void Update()
    {
    }

    public void SetButton(MinionType t, string desc)
    {
        _buttonText.text = t.ToString();
        minionPic.sprite = Resources.Load<Sprite>("UIMinionsPictures/" + _buttonText.text + "/" + _buttonText.text);
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
        padLock.enabled = true;
        SetNotBoughtColors();
    }

    public void UnlockButton()
    {
        if (!_isBlocked) return;

        _isBlocked = false;
        padLock.enabled = false;
    }

    void SetNotBoughtColors()
    {
        minionPic.color = unableToPurchaseColor;
        _buttonText.color = unableToPurchaseColor;
        _colorBlock.normalColor = unableToPurchaseColor;
        _colorBlock.highlightedColor = unableToPurchaseColor;
        button.colors = _colorBlock;
    }

    void SetBoughtColors()
    {
        _buttonText.color = minionPic.color = Color.white;
        _colorBlock.normalColor = Color.white;
        _colorBlock.highlightedColor = Color.white;
        button.colors = _colorBlock;
    }


    void MinionInShopClick()
    {
        onMinionClick(_description, _isBlocked, IsBought, minionType);
    }

    public void OnPointerDown()
    {
        _lastColor = _buttonText.color;
        _buttonText.color = nameTextColor;
    }

    public void OnPointerUp()
    {
        _buttonText.color = _lastColor;
    }
}
