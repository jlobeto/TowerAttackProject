using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryCanvasManager : MonoBehaviour
{
    public LibraryCategoryCanvas[] categoryScrollerCanvases;
    public LibraryTypeInfoMinionCanvas typeInfoMinionCanvas;
    Canvas _canvas;
    CategoryPanel _categoryPanel;
    PopupManager _popupManager;
    LibraryCategory _activeCategory = LibraryCategory.None;

    void Awake()
    {
        _popupManager = FindObjectOfType<PopupManager>();
        if (_popupManager == null)
            return;

        _categoryPanel = GetComponentInChildren<CategoryPanel>();

        _canvas = GetComponent<Canvas>();
        HideCanvas();
    }
    
    void Update()
    {
        
    }

    public void DisplayCanvas()
    {
        _canvas.enabled = true;
        _popupManager.PopupDisplayed();
        _categoryPanel.OnCategoryPressed += OnCategoryPressed;
    }

    public void HideCanvas()
    {
        _popupManager.DisplayedPopupWasClosed();
        _canvas.enabled = false;
        _categoryPanel.OnCategoryPressed -= OnCategoryPressed;

        foreach (var item in categoryScrollerCanvases)
            item.SetCanvas(false);
    }

    void OnCategoryPressed(LibraryCategory cat)
    {
        if(cat == LibraryCategory.None)
        {
            foreach (var item in categoryScrollerCanvases)
                if (item.IsCanvasActive())
                    item.SetCanvas(false);
        }
        else
        {
            foreach (var item2 in categoryScrollerCanvases)
            {
                if(item2.category == _activeCategory)
                    item2.SetCanvas(false);
                else if(item2.category == cat)
                    item2.SetCanvas(true);
            }
        }

        _activeCategory = cat;
    }
}
