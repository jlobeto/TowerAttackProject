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
    public string IsFirstIngameTutorialPartParams;
    public string AmountOfMinionsReleasedParams;
    #endregion
    TutorialManager _tutoManager;
    GameManager _gm;
    LevelTutorial _level;

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

    public bool IsLastTimerFinished()
    {
        return _tutoManager.TimerForNextGroupTutorialIsFinished;
    }

    public bool IsFirstIngameTutorialPart(string part)
    {
        int num = int.Parse(part);

        var lvl = GetTutorialLevelIfPossible();
        if (lvl == null) return false;

        return _level.CurrentTutorialPart + 1 == num;
    }

    public bool AmountOfMinionsReleased(string amount)
    {
        int num = int.Parse(amount);

        var lvl = GetTutorialLevelIfPossible();
        if (lvl == null) return false;

        return false;
    }

    public bool UserUsedMinionSkill()
    {
        var lvl = GetTutorialLevelIfPossible();
        if (lvl == null) return false;

        var result = lvl.phase3MinionSkillSelected;

        if (result)
            lvl.phase3MinionSkillSelected = false;

        return result;
    }

    LevelTutorial GetTutorialLevelIfPossible()
    {
        if (_level == null)
            _level = GameObject.FindObjectOfType<LevelTutorial>();
        
        return _level;
    }
}
