using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryCanvasManager : MonoBehaviour
{

    Canvas _canvas;
    CategoryPanel _categoryPanel;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _categoryPanel = GetComponentInChildren<CategoryPanel>();
        
    }
    
    void Update()
    {
        
    }

    public void DisplayCanvas()
    {
        _canvas.enabled = true;
    }

    public void HideCanvas()
    {
        _canvas.enabled = false;
    }
}
