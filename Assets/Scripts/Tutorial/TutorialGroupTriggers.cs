using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGroupTriggers : TutorialGroupUtils
{
    public string varFuncNames;//nombre de variables de funciones que tienen que ejecutarse
    
    #region Variables for pamams Storage
    public string IsSceneNameParams;
    #endregion
    TutorialManager _tutoManager;

    public TutorialGroupTriggers(TutorialManager t)
    {
        _tutoManager = t;
    }

    public bool CanTrigger()
    {
        var splittedFuncNames = varFuncNames.Split(' ');
        foreach (var item in splittedFuncNames)
        {
            var r = ExecuteFunction(item);

            if (r == null) continue;
            if (!(bool)r) return false;
        }

        return true;
    }

    public bool IsSceneName(string name)
    {
        var activeScene = SceneManager.GetActiveScene();
        var result = activeScene.name == name;
        return result;
    }

}
