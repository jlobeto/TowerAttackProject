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
    public string[] triggers;
    public string[] actions;
    public string[] outputs;//actions that happend after the user makes an input from the actions seen.
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
        ParseFunctionInput(ref _realTriggers, triggers);

        _realActions = new TutorialGroupActions(t, this);
        ParseFunctionInput(ref _realActions, actions);

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


    void ParseFunctionInput<T>(ref T obj, string[] input)
    {
        FieldInfo prop;

        for (int i = 0; i < input.Length; i++)
        {
            var split = input[i].Split('(');
            var funcName = split[0];
            var parameters = split[1].Length > 1 ? split[1].Substring(0, split[1].IndexOf(')')) : "";

            prop = obj.GetType().GetField("varFuncNames");
            prop.SetValue(obj, prop.GetValue(obj) + " " + funcName);

            if (parameters == "")
                continue;
            
            prop = obj.GetType().GetField(funcName+"Params");
            prop.SetValue(obj, parameters);
        }
    }

    void ParseOutputs<T>(ref T obj, string[] input)
    {
        if (input.Length == 0) return;

        FieldInfo field;
        foreach (var item in input)
        {
            var keyValue = item.Split('=');
            field = obj.GetType().GetField(keyValue[0]);
            field.SetValue(obj, keyValue[1]);
        }
    }
    
}
