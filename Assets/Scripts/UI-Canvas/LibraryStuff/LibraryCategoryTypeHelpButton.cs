using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryCategoryTypeHelpButton : LibraryCategoryTypeButton
{



    void Update()
    {
        
    }

    public override void Init(string type, LibraryCategory cat, Action<LibraryCategory, string> OnPressedCallback, GameManager gm)
    {
        _type = type;
        typeText.text = _type;
        _fromCategory = cat;

        _btn = GetComponentInChildren<Button>();
        _btn.onClick.AddListener(() => OnPressedCallback(_fromCategory, _type));
    }
}
