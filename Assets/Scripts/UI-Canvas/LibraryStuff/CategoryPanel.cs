using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CategoryPanel : MonoBehaviour
{
    public Image categoriesBG;
    public Text selectCategoryText;
    public Text selectTypeText;
    public List<CategoryButton> _categoryBtns;
    public Action<LibraryCategory> OnCategoryPressed = delegate { };

    LibraryCategory _currentSelected = LibraryCategory.None;

    void Start()
    {
        categoriesBG.enabled = false;

        foreach (CategoryButton item in _categoryBtns)
        {
            item.button.onClick.AddListener(() => OnCategoryButtonPressed(item.category));
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    
    void Update()
    {
        
    }

    void OnCategoryButtonPressed(LibraryCategory cat)
    {
        if(_currentSelected != LibraryCategory.None)
            GetButtonByType(_currentSelected).Unselect();

        if (_currentSelected == cat)
        {
            _currentSelected = LibraryCategory.None;
            categoriesBG.enabled = false;
            selectCategoryText.enabled = true;
            selectTypeText.enabled = false;
        }
        else
        {
            var btn = GetButtonByType(cat);
            btn.Select();
            _currentSelected = cat;
            if (!categoriesBG.IsActive())//is checking go and component so idk if this can be triggered
            {
                categoriesBG.enabled = true;
                selectCategoryText.enabled = false;
                selectTypeText.enabled = true;
            }

            categoriesBG.color = btn.categoryColor;
        }

        OnCategoryPressed(_currentSelected);
    }


    CategoryButton GetButtonByType(LibraryCategory cat)
    {
        foreach (var item in _categoryBtns)
        {
            if (item.category == cat)
                return item;
        }

        return null;
    }

    void OnSceneUnloaded(Scene scene)
    {
        //is this executing?
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        foreach (var item in _categoryBtns)
        {
            item.button.onClick.RemoveAllListeners();
        }
    }
}
