using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGroupUtils
{
    protected object ExecuteFunction(string funcName)
    {
        if (string.IsNullOrEmpty(funcName)) return null;

        var func = GetType().GetMethod(funcName);
        var parameters = GetType().GetField(funcName + "Params").GetValue(this).ToString();
        var splittedParams = parameters.Split(',');
        return func.Invoke(this, splittedParams);
    }

    protected GameObject GetParentByName(string name)
    {
        return GameObject.Find(name);
    }

    

}
