using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryButton : MonoBehaviour
{
    public Sprite notificationSprite;
    

    LibraryCanvasManager _libraryCanvas;
    Sprite _normalSprite;
    Image _btnImg;

    void Start()
    {
        _libraryCanvas = FindObjectOfType<LibraryCanvasManager>();

        _btnImg = GetComponent<Image>();
        _normalSprite = _btnImg.sprite;
    }

    
    void Update()
    {
        
    }

    public void DisplayLibrary()
    {
        _libraryCanvas.DisplayCanvas();
        gameObject.SetActive(false);
    }


    public void EnableNotification(bool enabled)
    {

    }
}
