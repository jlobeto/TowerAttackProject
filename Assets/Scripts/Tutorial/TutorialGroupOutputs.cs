using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class TutorialGroupOutputs : TutorialGroupUtils
{
    public string elementId;
    public string elementType;

    public string listeners = "";
    
    //examples                                                            ; => separate a func-type paramenter with its params  
    //"outputs": "PopupOnYes:PopupOnYesParams=HideBlackOverlay,ChangeScene;World Selector Screen,"

    TutorialManager _tutoManager;

    public TutorialGroupOutputs(TutorialManager t)
    {
        _tutoManager = t;
    }

    public void InitListeners()
    {
        if (listeners.Length == 0) return;

        SetListenerOfPopup();
        SetListenerOfButton();
    }

    void SetListenerOfPopup()
    {
        if (elementType != "AcceptPopup") return;

        var popup = GameObject.FindObjectsOfType<AcceptPopup>().FirstOrDefault(i => i.tutorialPopupID == elementId);
        var parsedListeners = ParseListeners();

        foreach (var listener in parsedListeners)
        {
            var enumType = GameUtils.ToEnum(listener.Item1, BasePopup.FunctionTypes.ok);

            foreach (var item in listener.Item2)
            {
                Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, item.Item1);
                popup.AddFunction(enumType, action, item.Item2);
            }
        }
    }


    

    void SetListenerOfButton()
    {
        if (elementType != "Button") return;

        //elementId will be the tag in the button.
        var button = GameObject.FindGameObjectWithTag(elementId).GetComponent<Button>();

        var parsedListeners = ParseListeners();
        foreach (var listener in parsedListeners)
        {
            foreach (var item in listener.Item2)
            {
                Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, item.Item1);
                button.onClick.AddListener(() => action(item.Item2));
            }
        }

    }

    public void HideBlackOverlay(string p)
    {
        var overlays = GameObject.FindObjectsOfType<Image>().Where(i => i.gameObject.name == TutorialManager.BLACK_OVERLAY_NAME);
        GameObject.Destroy(overlays.Last());
    }

    public void ChangeScene(string p)
    {
        SceneManager.LoadScene(p, LoadSceneMode.Single);
    }


    /// It does not remove it perse, it's sended to the original position and sibling index
    public void RemoveHighlightUIElement(string p)
    {
        var originalParentAndSiblingIndex = _tutoManager.LastUIParentAndSiblingIndex;
        var button = GameObject.FindGameObjectWithTag(elementId);

        button.transform.SetParent(originalParentAndSiblingIndex.Item1);
        button.transform.SetSiblingIndex(originalParentAndSiblingIndex.Item2);

    }


    //          type ,  list <Tuple< method,  params>
    List<Tuple<string, List<Tuple<MethodInfo, string>>>> ParseListeners()
    {
        var returnedList = new List<Tuple<string, List<Tuple<MethodInfo, string>>>>();
        List<Tuple<MethodInfo, string>> methods;
        var splittedListeners = listeners.Split('-');

        foreach (var listener in splittedListeners)
        {
            methods = new List<Tuple<MethodInfo, string>>();
            var indexOfColon = listener.IndexOf(':');
            var indexOfOpenBrackets = listener.IndexOf('{');
            var indexOfClosedBrackets = listener.IndexOf('}');
            var diff = indexOfOpenBrackets - indexOfColon - 1;

            var type = listener.Substring(indexOfColon + 1, diff);

            var functions = listener.Substring(indexOfOpenBrackets + 1, indexOfClosedBrackets - indexOfOpenBrackets - 1);

            foreach (var item in functions.Split(','))
            {
                string funcName = item;
                string p = "";
                var indexOfParenthesis = item.IndexOf('(');
                if (indexOfParenthesis >= 0)
                {
                    var indexOfParenthesisClosed = item.IndexOf(')');
                    funcName = funcName.Substring(0, indexOfParenthesis);
                    diff = indexOfParenthesisClosed - indexOfParenthesis;
                    p = item.Substring(indexOfParenthesis + 1, diff - 1);
                }

                methods.Add(Tuple.Create(GetType().GetMethod(funcName), p));
            }

            returnedList.Add(Tuple.Create(type, methods));
        }

        return returnedList;
    }
}
