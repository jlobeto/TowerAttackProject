using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button inside Category(eg: Runner, Drone, Tower Ice, etc)
/// </summary>
public class LibraryCategoryTypeButton : MonoBehaviour
{
    public Image typeImage;
    public Text typeText;

    protected string _type;
    protected LibraryCategory _fromCategory;
    protected Button _btn;


    void Awake()
    {
        
    }

    
    void Update()
    {
        
    }

    public virtual void Init(string type, LibraryCategory cat, Action<LibraryCategory, string> OnPressedCallback, GameManager gm)
    {
        Sprite sprite = gm.LoadedAssets.GetSpriteByName(type);

        if (sprite != null)
            typeImage.sprite = sprite;

        _type = type;
        typeText.text = _type;
        _fromCategory = cat;

        _btn = GetComponentInChildren<Button>();
        _btn.onClick.AddListener(() => OnPressedCallback(_fromCategory, _type));
    }
}
