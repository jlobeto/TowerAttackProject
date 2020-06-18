using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButton : MonoBehaviour
{
    public LibraryCategory category;
    public Color categoryColor;

    public Button button { get { return _btn; } }

    Button _btn;
    bool _isSelected;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    void Start()
    {
    }

    
    void Update()
    {
        
    }

    public void Select()
    {
        _isSelected = true;
        
    }

    public void Unselect()
    {
        _isSelected = false;
    }

}
