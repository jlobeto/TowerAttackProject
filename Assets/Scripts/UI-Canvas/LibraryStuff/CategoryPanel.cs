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

    public void SetDisabled()
    {
        if (_currentSelected != LibraryCategory.None)
            GetButtonByType(_currentSelected).Unselect();

        _currentSelected = LibraryCategory.None;

        selectCategoryText.enabled = true;

        categoriesBG.enabled = false;
        selectTypeText.enabled = false;
    }
    
    void OnCategoryButtonPressed(LibraryCategory cat)
    {
        if(_currentSelected != cat)
        {
            if(_currentSelected != LibraryCategory.None)
                GetButtonByType(_currentSelected).Unselect();

            var btn = GetButtonByType(cat);
            btn.Select();
            
            if (!categoriesBG.IsActive())//is checking go and component so idk if this can be triggered
            {
                categoriesBG.enabled = true;
                selectCategoryText.enabled = false;
                selectTypeText.enabled = true;
            }

            categoriesBG.color = btn.categoryColor;
            _currentSelected = cat;
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
