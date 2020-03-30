using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

[Serializable]
public class TutorialGroup
{
    public string tutorialGroupId;
    public string triggers;
    public string actions;
    public string resultingActions;
    
    TutorialGroupActions _realActions;
    TutorialGroupTriggers _realTriggers;

    
    public void SetTutorialGroup(TutorialManager t)
    {
        _realActions = new TutorialGroupActions(t);
        ParseFunctionInput(ref _realActions, actions.Split('/'));

        _realTriggers = new TutorialGroupTriggers(t);
        ParseFunctionInput(ref _realTriggers, triggers.Split('/'));
        
    }

    public bool CheckForTriggers()
    {
        return _realTriggers.CanTrigger();
    }

    public void ExecuteActions()
    {
        _realActions.ExecuteTutorialActions();
    }


    /// <summary>
    /// input[0] = "Func" > no cambia
    /// IsSceneName=IsScene:isSceneNameParams=MenuScreen,etc
    ///  funcname  = value :      params     =   values
    /// </summary>
    void ParseFunctionInput<T>(ref T obj, string[] input)
    {
        FieldInfo prop;

        for (int i = 0; i < input.Length; i++)
        {
            var splittedInfo = input[i].Split(':');
            prop = obj.GetType().GetField("varFuncNames");
            prop.SetValue(obj, prop.GetValue(obj) + " " + splittedInfo[0]);

            var splitParamNameFromValue = splittedInfo[1].Split('=');
            prop = obj.GetType().GetField(splitParamNameFromValue[0]);
            prop.SetValue(obj, splitParamNameFromValue[1]);

        }
    }













    /// <summary>
    /// input = "varname:value"
    /// input = "funcname:value:params/param1,param2,etc"
    /// </summary>
    void ParseInput<T>(ref T obj, string input)
    {
        var splittedInput = input.Split(' ');
        foreach (var item in splittedInput)
        {
            var valueKeySplit = item.Split(':');

            var prop = obj.GetType().GetField(valueKeySplit[0]);

            if (prop == null)
            {
                ParseFunctionInput(ref obj, valueKeySplit);
                continue;
            }

            if (valueKeySplit.Length == 2)
                prop.SetValue(obj, valueKeySplit[1]);
            else
                prop.SetValue(obj, true);
        }
    }
}
