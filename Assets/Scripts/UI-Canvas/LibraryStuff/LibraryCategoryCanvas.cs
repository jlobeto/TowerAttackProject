using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This manage the buttons inside each category
/// </summary>
public class LibraryCategoryCanvas : MonoBehaviour
{
    public LibraryCategory category;
    public LibraryCategoryTypeButton buttonTypePrefab;
    public RectTransform content;

    Canvas _canvas;
    LibraryCanvasManager _libraryCanvasManager;
    LibraryCategoryTypeButton[] _buttons;
    GameManager _gm;

    void Start()
    {
        if (_canvas == null)//maybe setCanvas() has been already called
            _canvas = GetComponent<Canvas>();

        _gm = FindObjectOfType<GameManager>();

        List<string> types = new List<string>();
        switch (category)
        {
            case LibraryCategory.Minions:
                types = Enum.GetNames(typeof(MinionType)).Where(i => i != MinionType.MiniZeppelin.ToString()).ToList();
                break;
            case LibraryCategory.Towers:
                types = Enum.GetNames(typeof(TowerType)).ToList();
                break;
            case LibraryCategory.Events:
                types = Enum.GetNames(typeof(EventTypes)).ToList();
                break;
            case LibraryCategory.Help:
                types = Enum.GetNames(typeof(LibraryHelpTypes)).ToList();
                break;
            default:
                break;
        }

        _buttons = new LibraryCategoryTypeButton[types.Count];
        LibraryCategoryTypeButton btn;
        for (int i = 0; i < types.Count; i++)
        {
            btn = Instantiate(buttonTypePrefab, content);
            btn.Init(types[i], category, OnCategoryTypeButtonPressed, _gm);
            _buttons[i] = btn;
        }

        btn = null;
    }

    public void Init(LibraryCanvasManager c)
    {
        _libraryCanvasManager = c;
    }

    public void SetCanvas(bool enable)
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();

        _canvas.enabled = enable;
    }

    public bool IsCanvasActive()
    {
        return _canvas.isActiveAndEnabled;
    }


    void OnCategoryTypeButtonPressed(LibraryCategory categoryPressed, string type)
    {
        SetCanvas(false);
        _libraryCanvasManager.OnCategoryTypeButtonPressed(categoryPressed, type);
    }
    
}
