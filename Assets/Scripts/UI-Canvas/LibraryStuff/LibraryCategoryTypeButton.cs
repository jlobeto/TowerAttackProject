using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibraryCategoryTypeButton : MonoBehaviour
{
    public Image typeImage;
    public Text typeText;

    string _type;
    LibraryCategory _fromCategory;


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Init(string imgPath, string type)
    {
        var sprite = Resources.Load<Sprite>(imgPath + "/" + type + "/" + type);
        if(sprite != null)
            typeImage.sprite = sprite;

        _type = type;
        typeText.text = _type;
    }
}
