using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGroupActions : TutorialGroupUtils
{
    public string varFuncNames;//nombre de variables de funciones que tienen que ejecutarse

    #region Variables for pamams Storage
    public string DisplayPopupParams;
    public string ShowBlackOverlayParams;
    public string HighlightUIElementParams;
    #endregion

    TutorialManager _tutoManager;

    public TutorialGroupActions(TutorialManager t)
    {
        _tutoManager = t;
    }

    public void ExecuteTutorialActions()
    {
        var splittedFuncNames = varFuncNames.Split(' ');
        foreach (var item in splittedFuncNames)
        {
            ExecuteFunction(item);
        }
    }


    public void DisplayPopup(string name, string title, string desc, string parent, string tutorialPopupID = "")
    {
        Canvas canvas = GetCanvasWithName(parent);

        if (name == "AcceptPopup")
        {
            var popup = GameObject.Instantiate(_tutoManager.acceptPopup, canvas.transform);
            popup.DisplayPopup();
            popup.title.text = title;
            popup.description.text = desc;
            popup.okButton.GetComponentInChildren<Text>().text = "Yes";
            popup.tutorialPopupID = tutorialPopupID;
        }
    }

    public void ShowBlackOverlay(string parent)
    {
        Canvas canvas = GetCanvasWithName(parent);
        var go = new GameObject();
        var img = go.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.8f);

        go.transform.SetParent(canvas.transform);

        img.rectTransform.anchorMin = new Vector2(0, 0);
        img.rectTransform.anchorMax = new Vector2(1, 1);
        img.rectTransform.offsetMin = new Vector2(0, 0);
        img.rectTransform.offsetMax = new Vector2(0, 0);

        go.name = TutorialManager.BLACK_OVERLAY_NAME;
    }


    public void HighlightUIElement(string elementsTag, string parent)
    {
        var canvas = GetCanvasWithName(parent);
        
        var r = canvas.GetComponentsInChildren<RectTransform>();
        RectTransform childTransform = null;
        foreach (var item in r)
        {
            if(item.CompareTag(elementsTag))
            {
                childTransform = item;
                break;
            }
        }

        _tutoManager.SetLastParentAndSibling(childTransform.parent, childTransform.GetSiblingIndex());

        if (childTransform.parent.name != parent)
        {
            
            childTransform.SetParent(canvas.transform);
        }
        else
        {
            childTransform.SetSiblingIndex(canvas.transform.childCount - 1);
        }

    }

    Canvas GetCanvasWithName(string name)
    {
        Canvas canvas = null;

        if (name == "Canvas")
            canvas = GameObject.FindObjectOfType<Canvas>();

        return canvas;
    }

}
