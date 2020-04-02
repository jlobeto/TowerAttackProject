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

    public string listeners;
    
    //examples                                                            ; => separate a func-type paramenter with its params  
    //"outputs": "PopupOnYes:PopupOnYesParams=HideBlackOverlay,ChangeScene;World Selector Screen,"

    TutorialManager _tutoManager;

    public TutorialGroupOutputs(TutorialManager t)
    {
        _tutoManager = t;
    }

    public void InitListeners()
    {
        if (elementType == "AcceptPopup")
        {
            var popup = GameObject.FindObjectsOfType<AcceptPopup>().FirstOrDefault(i => i.tutorialPopupID == elementId);

            MethodInfo methodInfo;
            var splittedListeners = listeners.Split('-');
            foreach (var listener in splittedListeners)
            {
                var indexOfColon = listener.IndexOf(':');
                var indexOfOpenBrackets = listener.IndexOf('{');
                var indexOfClosedBrackets = listener.IndexOf('}');
                var diff = indexOfOpenBrackets - indexOfColon - 1;

                var type = listener.Substring(indexOfColon+1, diff);

                var parameters = listener.Substring(indexOfOpenBrackets+1, indexOfClosedBrackets - indexOfOpenBrackets - 1);

                var enumType = GameUtils.ToEnum(type, BasePopup.FunctionTypes.ok);
                foreach (var item in parameters.Split(','))
                {
                    string funcName = item;
                    string p = "";
                    var indexOfParenthesis = item.IndexOf('(');
                    if (indexOfParenthesis >= 0)
                    {
                        var indexOfParenthesisClosed = item.IndexOf(')');
                        funcName = funcName.Substring(0, indexOfParenthesis);
                        diff = indexOfParenthesisClosed - indexOfParenthesis;
                        p = item.Substring(indexOfParenthesis+1, diff-1);
                    }

                    methodInfo = GetType().GetMethod(funcName);
                    Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, methodInfo);
                    popup.AddFunction(enumType, action, p);
                }
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

}
