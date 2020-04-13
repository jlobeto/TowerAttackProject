using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public string LockScrollRectMoveParams;
    public string SetUIElementListsTransparencyParams;
    #endregion

    TutorialManager _tutoManager;
    TutorialGroup _tutoGroup;

    public TutorialGroupActions(TutorialManager t, TutorialGroup g)
    {
        _tutoManager = t;
        _tutoGroup = g;
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
        var canvas = GetParentByName(parent);

        if (name == "AcceptPopup")
        {
            var popup = GameObject.Instantiate(_tutoManager.acceptPopup, canvas.transform);
            popup.DisplayPopup();
            popup.title.text = title;
            popup.description.text = desc;
            popup.okButton.GetComponentInChildren<Text>().text = "Yes";
            popup.closeButton.GetComponentInChildren<Text>().text = "No";
            popup.tutorialPopupID = tutorialPopupID;
            popup.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ShowBlackOverlay(string parent)
    {
        var canvas = GetParentByName(parent);
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


    public void HighlightUIElement(string elementName, string parent)
    {
        var parentGO = GetParentByName(parent);

        /*var r = canvas.GetComponentsInChildren<RectTransform>();
        foreach (var item in r)
        {
            if(item.CompareTag(elementsTag))
            {
                childTransform = item;
                break;
            }
        }*/
        Transform childTransform = GameObject.Find(elementName).transform;

        _tutoManager.SetLastParentAndSibling(childTransform.parent, childTransform.GetSiblingIndex());

        if (childTransform.parent.name != parent)
        {
            
            childTransform.SetParent(parentGO.transform);
        }
        else
        {
            childTransform.SetSiblingIndex(parentGO.transform.childCount - 1);
        }

    }
    
    public void LockScrollRectMove(string elementName, string parent)
    {
        var canvas = GetParentByName(parent);
        ScrollRect rect = GameObject.Find(elementName).GetComponent<ScrollRect>();
        rect.horizontal = rect.vertical = false;
    }

    public void SetUIElementListsTransparency(string elementName, string exception, string alphaPercent)
    {
        //hide all scroll rect elements except for 'exception'
        GameObject groupElement = GameObject.Find(elementName);
        foreach (Transform item in groupElement.transform)
        {
            if (item.name != exception)
            {
                var alpha = int.Parse(alphaPercent);
                var img = item.GetComponent<Image>();
                var txt = item.GetComponent<Text>();
                if (img != null)
                    img.color = new Color(img.color.r, img.color.g, img.color.b, alpha * 1.0f / 255);
                if (txt != null)
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha * 1.0f / 255);

                SetTransparentToChildren(item.gameObject, alpha);
            }
        }
    }

    private void SetTransparentToChildren(GameObject element, int alpha)
    {
        var childrenImg = element.GetComponentsInChildren<Image>();
        var childrenTxt = element.GetComponentsInChildren<Text>();
        foreach (var img in childrenImg)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha * 1.0f / 255);
        }

        foreach (var txt in childrenTxt)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha * 1.0f / 255);
        }
    }
}
