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
        var field = GetType().GetField(funcName + "Params");
        var splittedParams = new string[0];

        if (field != null)
        {
            var parameters = field.GetValue(this).ToString();
            splittedParams = parameters.Split(',');
        }
        
        return func.Invoke(this, splittedParams);
    }

    protected GameObject GetParentByName(string name)
    {
        return GameObject.Find(name);
    }

    

}
