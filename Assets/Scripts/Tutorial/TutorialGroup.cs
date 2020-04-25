using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;

[Serializable]
public class TutorialGroup
{
    public string tutorialPhase;
    public string tutorialGroupId;
    public string triggers;
    public string actions;
    public string outputs;//actions that happend after the user makes an input from the actions seen.
    public Action OnTutorialFinished = delegate { };
    public List<Image> pointingFingers;//using this to destroy it in outputs.

    TutorialPhase _phase;
    TutorialGroupActions _realActions;
    TutorialGroupTriggers _realTriggers;
    TutorialGroupOutputs _realOutputs;
    TutorialManager _tutoManager;
    
    public void SetTutorialGroup(TutorialManager t, GameManager gm)
    {
        pointingFingers = new List<Image>();
        _tutoManager = t;

        _realTriggers = new TutorialGroupTriggers(t, gm);
        ParseFunctionInput(ref _realTriggers, triggers.Split('/'));

        _realActions = new TutorialGroupActions(t, this);
        ParseFunctionInput(ref _realActions, actions.Split('/'));

        _realOutputs = new TutorialGroupOutputs(t, this);
        ParseOutputs(ref _realOutputs, outputs);

        _phase = GameUtils.ToEnum(tutorialPhase, TutorialPhase.FirstTimeOnApp);
    }

    public bool CheckForTriggers()
    {
        var isLastGroupThisGroup = _tutoManager.LastTutorialGroupId == tutorialGroupId;
        if (isLastGroupThisGroup) return false;

        return _realTriggers.CanTrigger();
    }

    public void ExecuteActions()
    {
        _realActions.ExecuteTutorialActions();
        _realOutputs.InitListeners();
        _tutoManager.LastTutorialGroupId = tutorialGroupId;
    }

    public void OnOutputFinished()
    {
        OnTutorialFinished();
    }

    public void OnPhaseDoneOrCanceled()
    {
        _tutoManager.TutorialFinished(_phase);
    }

    /// <summary>
    /// input[0] = "Func" > no cambia
    /// IsSceneName=IsScene:isSceneNameParams=MenuScreen,etc
    ///  funcname  = value :      params     =   values
    /// </summary>
    void ParseFunctionInput<T>(ref T obj, string[] input)
    {
        /*/1- PopupOnYes:PopupOnYesParams=HideBlackOverlay,ChangeScene;World Selector Screen
                A) PopupOnYes
                B) PopupOnYesParams=HideBlackOverlay,ChangeScene;World Selector Screen
                    - PopupOnYesParams
                    - HideBlackOverlay,ChangeScene;World Selector Screen
        
           2- PopupOnNo:PopupOnNoParams=HideBlackOverlay
         */
        FieldInfo prop;

        for (int i = 0; i < input.Length; i++)
        {
            var splittedInfo = input[i].Split(':');
            prop = obj.GetType().GetField("varFuncNames");
            prop.SetValue(obj, prop.GetValue(obj) + " " + splittedInfo[0]);

            if (splittedInfo.Length == 1)
                continue;

            var splitParamNameFromValue = splittedInfo[1].Split('=');
            prop = obj.GetType().GetField(splitParamNameFromValue[0]);
            prop.SetValue(obj, splitParamNameFromValue[1]);

        }
    }

    void ParseOutputs<T>(ref T obj, string input)
    {
        if (input.Length == 0) return;

        FieldInfo field;
        var split = input.Split(';');
        foreach (var item in split)
        {
            var keyValue = item.Split('=');
            field = obj.GetType().GetField(keyValue[0]);
            field.SetValue(obj, keyValue[1]);
        }
    }




    
}
