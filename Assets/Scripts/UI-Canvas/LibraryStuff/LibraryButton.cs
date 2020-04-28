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
    Text _txt;

    void Start()
    {
        _libraryCanvas = FindObjectOfType<LibraryCanvasManager>();

        _txt = GetComponentInChildren<Text>();
        _txt.text = "!";
        _txt.enabled = false;

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

    public void HideLibrary()
    {
        _libraryCanvas.HideCanvas();
        gameObject.SetActive(true);
    }

    public void EnableNotification(bool enabled)
    {
        if(enabled)
        {
            _btnImg.sprite = notificationSprite;
            _txt.enabled = true;
        }
        else
            _btnImg.sprite = _normalSprite;
    }
}
