using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CategoryPanel : MonoBehaviour
{
    public Image categoriesBG;
    public Text selectCategoryText;

    List<CategoryButton> _categoryBtns;
    LibraryCategory _currentSelected = LibraryCategory.None;

    void Start()
    {
        categoriesBG.enabled = false;

        _categoryBtns = GetComponentsInChildren<CategoryButton>().ToList();
        foreach (CategoryButton item in _categoryBtns)
        {
            item.button.onClick.AddListener(() => OnCategoryPressed(item.category));
        }

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    
    void Update()
    {
        
    }

    public void OnCategoryPressed(LibraryCategory cat)
    {
        GetButtonByType(_currentSelected).Unselect();

        if (_currentSelected == cat)
        {
            _currentSelected = LibraryCategory.None;
            categoriesBG.enabled = false;
            selectCategoryText.enabled = true;
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
            }
                
            categoriesBG.color = btn.categoryColor;
        }
            
    }


    CategoryButton GetButtonByType(LibraryCategory cat)
    {
        return _categoryBtns.FirstOrDefault(i => i.category == cat);
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
