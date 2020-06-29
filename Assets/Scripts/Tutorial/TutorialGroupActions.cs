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
    public string ForceExecutingOutputAfterSecondsParams;
    public string CreatePointFingerParams;
    public string ShowPointFingerInSceneParams;
    public string SetMinionsAndTowersParams;
    public string BlockOrNotSingleButtonParams;
    public string ShowTextParams;
    public string ShopDocUIParams;
    #endregion

    TutorialManager _tutoManager;
    TutorialGroup _tutoGroup;
    DocUICanvas _docUICanvas;

    public TutorialGroupActions(TutorialManager t, TutorialGroup g)
    {
        _tutoManager = t;
        _tutoGroup = g;
        _docUICanvas = GameObject.FindObjectOfType<DocUICanvas>();
    }

    public void ExecuteTutorialActions()
    {
        var splittedFuncNames = varFuncNames.Split(' ');
        foreach (var item in splittedFuncNames)
        {
            ExecuteFunction(item);
        }
    }

    public void ShopDocUI(string position, string text, string mood, string scale)
    {
        var m = GameUtils.ToEnum(mood, DocUICanvas.DocUIMood.normal);
        var pos = GameUtils.ToEnum(position, DocUICanvas.DocUIPosition.bottomLeft);
        var realS = float.Parse(scale);

        _docUICanvas.ShowDocUI(m, text, pos, realS);
    }


    public void DisplayPopup(string name, string title, string desc, string parent, string tutorialPopupID = "")
    {
        var canvas = GetGameObjectByName(parent);

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
        var canvasGO = GetGameObjectByName(parent);
        var canvas = canvasGO.GetComponent<Canvas>();
        var existingOne = GetGameObjectByName(TutorialManager.BLACK_OVERLAY_NAME);

        if (existingOne != null)
            GameObject.Destroy(existingOne);

        var go = new GameObject(TutorialManager.BLACK_OVERLAY_NAME);
        var img = go.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.8f);
        go.transform.SetParent(canvasGO.transform);

        img.rectTransform.anchorMin = new Vector2(0, 0);
        img.rectTransform.anchorMax = new Vector2(1, 1);
        img.rectTransform.offsetMin = new Vector2(0, 0);
        img.rectTransform.offsetMax = new Vector2(0, 0);

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            img.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
            img.rectTransform.localScale = new Vector3(1, 1, 1);
            img.rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);
        }
    }

    public void ShowText(string text, string posX, string posY, string parent, string size)
    {
        /*this will create the text.
         - parent is the game object that the finger will be attached to
         - the positions x and y will be added from the pivot of the parent*/

        var parentGO = GetGameObjectByName(parent);
        Text txt = _tutoManager.tutorialText;
        if (txt == null)
        {
            var go = new GameObject(TutorialManager.TUTORIAL_TEXT_NAME);
            txt = go.AddComponent<Text>();
            txt.font = _tutoManager.texts_font;
            txt.color = Color.white;
            _tutoManager.tutorialText = txt;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.supportRichText = false;
            txt.raycastTarget = false;
            txt.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 500);
            txt.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 400);
        }
        else if(!txt.isActiveAndEnabled)
        {
            txt.enabled = true;
        }

        if (txt.color.a < 1f)
            txt.color = new Color(255, 255, 255, 255);

        var x = float.Parse(posX);
        var y = float.Parse(posY);

        txt.rectTransform.SetParent(parentGO.transform);
        txt.rectTransform.localPosition = new Vector3();
        txt.rectTransform.localPosition += new Vector3(x, y, 0);
        txt.rectTransform.localScale = new Vector3(1, 1, 1);

        txt.fontSize = int.Parse(size);
        txt.text = text;

        var canvas = GetGameObjectByName("LevelCanvas");
        if (canvas != null)
        {
            var c = canvas.GetComponent<Canvas>();
            if (c.renderMode != RenderMode.ScreenSpaceCamera) return;

            txt.rectTransform.localPosition += new Vector3(x, y, 0);
            txt.rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
            txt.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }


    public void HighlightUIElement(string elementName, string parent)
    {
        var parentGO = GetGameObjectByName(parent);
        
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

    public void DisplayTuto2Fingers()
    {
        var level = GameObject.FindObjectOfType<LevelTutorial>();
        foreach (var item in level.fingersPhase2)
        {
            item.gameObject.SetActive(true);
            item.rectTransform.SetSiblingIndex(item.rectTransform.parent.childCount-1);
        }
        level.levelPortal.SetPS(false);
    }
    
    public void StopUpdate()
    {
        Time.timeScale = 0;
    }
    
    public void LockScrollRectMove(string elementName, string parent)
    {
        var canvas = GetGameObjectByName(parent);
        ScrollRect rect = GameObject.Find(elementName).GetComponent<ScrollRect>();
        rect.horizontal = rect.vertical = false;
    }

    public void ForceExecutingOutputAfterSeconds(string milliseconds)
    {
        var sec = float.Parse(milliseconds) / 1000;
        _tutoManager.ForceExecutingOutputAfterSeconds(sec);
    }
    

    public void CreatePointFinger(string posX, string posY, string rotZ, string parent)
    {
        /*this will create a pointing finger.
         - parent is the game object that the finger will be attached to
         - the positions x and y will be added from the pivot of the parent*/
        var parentGO = GetGameObjectByName(parent);

        if (posX.Contains("parentW"))//will get the parent width and get the middle of it 
        {
            var parentRect = parentGO.GetComponent<RectTransform>();
            posX = (parentRect.rect.width / 2).ToString("0.00");
        }

        var x = float.Parse(posX);
        var y = float.Parse(posY);
        var rotatZ = float.Parse(rotZ);

        var finger = GameObject.Instantiate<Image>(_tutoManager.pointingFinger, new Vector3(0,0,0), Quaternion.identity, parentGO.transform);
        finger.rectTransform.position = parentGO.transform.position;
        finger.name = TutorialManager.POINTING_FINGER_NAME;

        finger.rectTransform.position += new Vector3(x, y, 0);
        finger.rectTransform.Rotate(new Vector3(0, 0, rotatZ), Space.Self);

        finger.raycastTarget = false;
        _tutoGroup.pointingFingers.Add(finger);

        var canvas = GetGameObjectByName("LevelCanvas");
        if(canvas != null)
        {
            var c = canvas.GetComponent<Canvas>();
            if (c.renderMode != RenderMode.ScreenSpaceCamera) return;

            finger.rectTransform.localPosition = new Vector3(0, 0, 0) + new Vector3(x,y,0);
            finger.rectTransform.localRotation = new Quaternion(0, 0, 0, 0);
            finger.rectTransform.Rotate(new Vector3(0, 0, rotatZ), Space.Self);
        }
    }

    public void SetMinionsAndTowers(string val)
    {
        var value = bool.Parse(val);
        var lvl = GameObject.FindObjectOfType<LevelTutorial>();
        lvl.SetMinionsAndTowers(value);
    }

    public void BlockOrNotSingleButton(string elementName, string value, string alpha)
    {
        GameObject element = GameObject.Find(elementName);
        if (element == null)
            Debug.LogError("TUTORIAL ACTION > " + elementName + " does not exist");

        var btn = element.GetComponent<Button>();
        var img = element.GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, int.Parse(alpha)* 1.0f / 255);

        var childImg = element.GetComponentInChildren<Image>();
        if(childImg != null)
            childImg.color = new Color(childImg.color.r, childImg.color.g, childImg.color.b, int.Parse(alpha) * 1.0f / 255);

        btn.interactable = bool.Parse(value);
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
                var btn = item.GetComponent<Button>();

                if (img != null)
                    img.color = new Color(img.color.r, img.color.g, img.color.b, alpha * 1.0f / 255);
                if (txt != null)
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha * 1.0f / 255);
                if (btn != null)
                    btn.interactable = alpha == 255;

                SetTransparentToChildren(item.gameObject, alpha);
            }
        }
    }

    private void SetTransparentToChildren(GameObject element, int alpha)
    {
        var childrenImg = element.GetComponentsInChildren<Image>();
        var childrenTxt = element.GetComponentsInChildren<Text>();
        var childrenBtn = element.GetComponentsInChildren<Button>();

        foreach (var img in childrenImg)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha * 1.0f / 255);
        }

        foreach (var txt in childrenTxt)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, alpha * 1.0f / 255);
        }

        foreach (var btn in childrenBtn)
        {
            btn.interactable = alpha == 255;
        }
    }
}
