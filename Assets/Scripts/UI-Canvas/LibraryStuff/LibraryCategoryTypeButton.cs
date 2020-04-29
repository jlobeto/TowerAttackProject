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

    string _type;
    LibraryCategory _fromCategory;
    Button _btn;


    void Start()
    {
        _btn = GetComponentInChildren<Button>();
    }

    
    void Update()
    {
        
    }

    public void Init(string imgPath, string type, LibraryCategory cat, Action<LibraryCategory, string> OnPressedCallback = null)
    {
        var sprite = Resources.Load<Sprite>(imgPath + "/" + type + "/" + type);
        if(sprite != null)
            typeImage.sprite = sprite;

        _type = type;
        typeText.text = _type;
        _fromCategory = cat;

        //_btn.onClick.AddListener(() => OnPressedCallback(_fromCategory, _type));
    }
}
