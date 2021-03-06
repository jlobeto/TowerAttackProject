﻿using System.Collections;
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

        //Debug.Log(funcName + " " + splittedParams.Length);
        if (funcName == "ShowDocUI")
        {
            foreach (var item in splittedParams)
            {
                //Debug.Log(item);
            }
        }
        

        return func.Invoke(this, splittedParams);
    }

    protected GameObject GetGameObjectByName(string name)
    {
        return GameObject.Find(name);
    }

    

}
