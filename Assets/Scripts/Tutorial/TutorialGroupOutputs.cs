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
    
    TutorialManager _tutoManager;
    TutorialGroup _tutoGroup;
    Dictionary<string, int> _amountOfFunctionPerListener;
    Dictionary<string, int> _currentAmountOfFunctionsTriggeredPerListener;

    public TutorialGroupOutputs(TutorialManager t, TutorialGroup tutoGroup)
    {
        _tutoManager = t;
        _tutoGroup = tutoGroup;

        _amountOfFunctionPerListener = new Dictionary<string, int>();
        _currentAmountOfFunctionsTriggeredPerListener = new Dictionary<string, int>();
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
            int amount = 0;
            foreach (var item in listener.Item2)
            {
                Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, item.Item1);
                string p = item.Item2 + "/" + listener.Item1;
                popup.AddFunction(enumType, action, p);
                amount++;
            }

            _amountOfFunctionPerListener.Add(listener.Item1, amount);
        }
    }


    

    void SetListenerOfButton()
    {
        if (elementType != "Button") return;

        //elementId will be the name in the button.
        var button_go = GameObject.Find(elementId);
        var button = button_go.GetComponent<Button>();
        if(button == null)
            button = button_go.GetComponentInChildren<Button>();

        var parsedListeners = ParseListeners();
        foreach (var listener in parsedListeners)
        {
            int amount = 0;
            foreach (var item in listener.Item2)
            {
                var p = item.Item2 + "/"+ listener.Item1;
                Action<string> action = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), this, item.Item1);
                button.onClick.AddListener(() => action(p));
                amount++;
            }
            _amountOfFunctionPerListener.Add(listener.Item1, amount);
        }
    }

    public void HideBlackOverlay(string p)
    {
        
        var overlays = GameObject.FindObjectsOfType<Image>().Where(i => i.gameObject.name == TutorialManager.BLACK_OVERLAY_NAME);
        GameObject.Destroy(overlays.Last().gameObject);
        var split = p.Split('/');
        OnFuncFinished(split[split.Length - 1]);
    }

    public void ChangeScene(string p)
    {
        var split = p.Split('/');
        SceneManager.LoadScene(split[0], LoadSceneMode.Single);
    }

    public void ForceTutorialGroupToFinish(string p)
    {
        var split = p.Split('/');
        OnFuncFinished(split[1]);
    }


    /// It does not remove it perse, it's sended to the original position and sibling index
    public void RemoveHighlightUIElement(string p)
    {
        var split = p.Split('/');
        var originalParentAndSiblingIndex = _tutoManager.LastUIParentAndSiblingIndex;
        var button = GameObject.Find(split[0]);

        button.transform.SetParent(originalParentAndSiblingIndex.Item1);
        button.transform.SetSiblingIndex(originalParentAndSiblingIndex.Item2);

        
        OnFuncFinished(split[split.Length - 1]);
    }

    public void UnlockScrollRectMove(string p)
    {
        var split = p.Split('/');

        var canvas = GetParentByName(split[1]);//Canvas
        ScrollRect rect = GameObject.Find(split[0]).GetComponent<ScrollRect>();//ScrollRect name
        rect.horizontal = true;

        OnFuncFinished(split[split.Length - 1]);
    }

    public void SetUIElementListsTransparency(string p)
    {
        var split = p.Split('/');

        string elementName = split[0];
        string exception = split[1];
        string alphaPercent = split[2];
        string funcType = split[3];

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
        OnFuncFinished(funcType);
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


    void OnFuncFinished(string type)
    {
        if (!_currentAmountOfFunctionsTriggeredPerListener.ContainsKey(type))
            _currentAmountOfFunctionsTriggeredPerListener.Add(type, 0);

        _currentAmountOfFunctionsTriggeredPerListener[type]++;

        if(_currentAmountOfFunctionsTriggeredPerListener[type] == _amountOfFunctionPerListener[type])
            _tutoGroup.OnOutputFinished();
    }


    public void OnAgreeToDoTutorial(string p)
    {
        _tutoManager.UserAgreesWithDoFirstTutorial(true);
        _tutoManager.FirstTimeAppIsOpened();
    }

    public void OnCancelToDoTutorial(string p)
    {
        _tutoManager.UserAgreesWithDoFirstTutorial(false);
        _tutoManager.FirstTimeAppIsOpened();
        _tutoGroup.OnPhaseDoneOrCanceled();
    }

    public void OnPhaseCompleted(string p)
    {
        _tutoGroup.OnPhaseDoneOrCanceled();
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
