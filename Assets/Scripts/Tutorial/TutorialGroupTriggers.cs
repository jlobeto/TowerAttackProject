using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGroupTriggers : TutorialGroupUtils
{
    public string varFuncNames;//nombre de variables de funciones que tienen que ejecutarse
    
    #region Variables for pamams Storage
    public string IsSceneNameParams;
    public string IsPreviousSceneParams;
    public string IsPreviousTutorialGroupParams;
    public string HasUserNotCompletedTutorialParams;
    #endregion
    TutorialManager _tutoManager;
    GameManager _gm;

    public TutorialGroupTriggers(TutorialManager t, GameManager gm)
    {
        _tutoManager = t;
        _gm = gm;
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

    public bool IsPreviousScene(string name)
    {
        return _gm.LastLoadedScene == name;
    }

    public bool IsPreviousTutorialGroup(string id)
    {
        return _tutoManager.LastTutorialGroupId == id;
    }

    public bool HasUserNotCompletedTutorial(string phase)
    {
        return !_tutoManager.HasUserCompletedTutorial(phase);
    }
}
