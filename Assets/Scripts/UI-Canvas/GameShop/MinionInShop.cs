using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinionInShop : MonoBehaviour
{

    public Button button;
    public Text buttonText;
    public Image minionPic;
    public Image padLock;
    public Image padLockOpen;
    public Action<string, bool, bool, MinionType> onMinionClick = delegate { };
    public MinionType minionType;
    public Color unableToPurchaseColor;
    public Color nameTextColor;
    public Color selectedColor;

    bool _isBought;
    bool _isBlocked;
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
        button.onClick.AddListener(MinionInShopClick);
        _colorBlock = button.colors;
        var pointer = button.GetComponent<OnCustomPointerCallback>();
        pointer.AddListener(OnCustomPointerCallback.Listener.pointerDown, OnPointerDown);
        pointer.AddListener(OnCustomPointerCallback.Listener.pointerUp, OnPointerUp);

        padLock.enabled = false;
        padLockOpen.enabled = false;
    }


    void Update()
    {
    }

    public void SetButton(MinionType t, string desc, GameManager gm)
    {
        buttonText.text = t.ToString();
        minionPic.sprite =  gm.LoadedAssets.GetSpriteByName(buttonText.text);
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
        padLockOpen.enabled = false;
        SetNotBoughtColors();
    }

    public void UnlockButton()
    {
        if (!_isBlocked) return;

        _isBlocked = false;
        padLock.enabled = false;

        padLockOpen.enabled = true;
    }

    public void ChangeToColor(bool isSelected)
    {
        if (isSelected)
        {
            _colorBlock.normalColor = selectedColor;
            _colorBlock.highlightedColor = selectedColor;
            button.colors = _colorBlock;

            buttonText.color = selectedColor;
        }
        else
        {
            if (_isBlocked/* || !_isBought*/)
                SetNotBoughtColors();
            else
                SetBoughtColors();
        }
    }

    void SetNotBoughtColors()
    {
        minionPic.color = unableToPurchaseColor;
        buttonText.color = unableToPurchaseColor;
        _colorBlock.normalColor = unableToPurchaseColor;
        _colorBlock.highlightedColor = unableToPurchaseColor;
        button.colors = _colorBlock;


        if (!_isBought && !_isBlocked)
            padLockOpen.enabled = true;
    }

    void SetBoughtColors()
    {
        if (buttonText == null) return;
        buttonText.color = minionPic.color = Color.white;
        _colorBlock.normalColor = Color.white;
        _colorBlock.highlightedColor = Color.white;
        button.colors = _colorBlock;

        if (_isBought)
            padLockOpen.enabled = false;
    }


    void MinionInShopClick()
    {
        onMinionClick(_description, _isBlocked, IsBought, minionType);
    }

    public void OnPointerDown()
    {
        if(button.IsInteractable())
        {
            _lastColor = buttonText.color;
            buttonText.color = nameTextColor;
        }
        
    }

    public void OnPointerUp()
    {
        if(button.IsInteractable())
            buttonText.color = _lastColor;
    }
}
