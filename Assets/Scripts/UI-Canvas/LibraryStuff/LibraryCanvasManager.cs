﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryCanvasManager : MonoBehaviour
{
    public const string SELECT_ONE_TYPE_TEXT = "select one type";

    public CategoryPanel categoryPanel;
    public LibraryCategoryCanvas[] categoryScrollerCanvases;
    public LibraryTypeInfoCanvas typeInfoCanvas;
    public Text exitButtonText;

    Canvas _canvas;
    CategoryPanel _categoryPanel;
    PopupManager _popupManager;
    GameManager _gameManager;
    LibraryCategory _activeCategory = LibraryCategory.None;
    bool _isDisplayed;


    void Awake()
    {
        _popupManager = FindObjectOfType<PopupManager>();
        if (_popupManager == null)
            return;
        
        _gameManager = FindObjectOfType<GameManager>();
        _categoryPanel = GetComponentInChildren<CategoryPanel>();

        foreach (var item in categoryScrollerCanvases)
            item.Init(this);

        _canvas = GetComponent<Canvas>();
        HideCanvas();
    }
    
    void Update()
    {
        
    }

    public void DisplayCanvas()
    {
        _isDisplayed = true;
        _canvas.enabled = true;
        _popupManager.PopupDisplayed();
        _categoryPanel.OnCategoryPressed += OnCategoryPressed;

        Time.timeScale = 0;
    }

    public void HideCanvas()
    {
        if(_isDisplayed)
            _popupManager.DisplayedPopupWasClosed();

        _canvas.enabled = false;
        _categoryPanel.OnCategoryPressed -= OnCategoryPressed;

        categoryPanel.SetDisabled();

        foreach (var item in categoryScrollerCanvases)
            item.SetCanvas(false);

        typeInfoCanvas.SetCanvas(false);
        _isDisplayed = false;
        Time.timeScale = 1;
    }

    public void OnCategoryTypeButtonPressed(LibraryCategory categoryPressed, string typePressed)
    {
        var data = _gameManager.LibraryManager.GetLibraryData(categoryPressed, typePressed);

        typeInfoCanvas.SetInfo(data);

        exitButtonText.text = "back";
        categoryPanel.selectTypeText.text = typePressed;
    }

    /// <summary>
    /// It can be the exit or the return button
    /// </summary>
    public void OnExitButtonPressed()
    {
        if(typeInfoCanvas.isShowingInfo)
        {
            OnCategoryPressed(_activeCategory);
            categoryPanel.selectTypeText.text = SELECT_ONE_TYPE_TEXT;
        }
        else
        {
            HideCanvas();
        }

        SoundManager.instance.PlaySound(SoundFxNames.back_button);
    }

    void OnCategoryPressed(LibraryCategory cat)
    {
        typeInfoCanvas.SetCanvas(false);
        //Delete bottom lines when all scroller are implemented:
        var current = categoryScrollerCanvases.FirstOrDefault(i => i.category == _activeCategory);
        if (current != null) current.SetCanvas(false);

        foreach (var item in categoryScrollerCanvases)
        {
            if (item.category == cat && !item.IsCanvasActive())
            {
                current = categoryScrollerCanvases.FirstOrDefault(i => i.category == _activeCategory);
                if (current != null) current.SetCanvas(false);

                item.SetCanvas(true);
            }
        }

        exitButtonText.text = "exit";

        _activeCategory = cat;
        categoryPanel.selectTypeText.text = SELECT_ONE_TYPE_TEXT;
    }
}
