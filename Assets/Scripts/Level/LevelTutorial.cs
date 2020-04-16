using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : Level
{
    public int[] objetives1;
    public int[] objetives2;
    public int[] objetives3;
    public GameObject _towersPhase2;
    public GameObject _towersPhase3;

    List<int[]> _objectiveList;
    int _currentTutorial = 0;

    protected override void Init()
    {
        base.Init();
        _objectiveList = new List<int[]>(){objetives1, objetives2, objetives3};

        objetives = _objectiveList[_currentTutorial];
        _lvlCanvasManager.EnableDisableMinionSkillButtons(false);
    }

    protected override void GoalCompletedHandler(bool shouldShowPopup = true)
    {
        if (_gameManager.popupManager != null)
        {
            BasePopup popup = null;
            if(_currentTutorial+1 >= _objectiveList.Count)
            {
                popup = _gameManager.popupManager.BuildPopup(_lvlCanvasManager.transform, "TUTORIAL COMPLETED!", "Continue tu main map", "Continue");
                popup.AddFunction(BasePopup.FunctionTypes.ok, OnFinishLevelCallback);
                base.GoalCompletedHandler(false);
            }
            else
            {
                var phaseTxt = "PHASE " + (_currentTutorial + 1) + "/" + _objectiveList.Count;
                popup = _gameManager.popupManager.BuildPopup(_lvlCanvasManager.transform
                    ,phaseTxt + " COMPLETED!"
                    , "Do you want to continue with the tutorial ?"
                    , "Continue"
                    , PopupsID.AcceptOrDecline);

                popup.AddFunction(BasePopup.FunctionTypes.ok, setNextTutorialIfPossible);
                popup.AddFunction(BasePopup.FunctionTypes.cancel, cancelTutorial);
            }

            popup.transform.localScale = new Vector3(.7f, .7f, .7f);
        }
    }

    void setNextTutorialIfPossible(string p)//parameter does not matter
    {
        _currentTutorial++;
        if (_currentTutorial == 1)//phase 2
            _towersPhase2.SetActive(true);
        else
        {
            _towersPhase3.SetActive(true);
            _towersPhase2.SetActive(false);
            _lvlCanvasManager.EnableDisableMinionSkillButtons(true);
        }

        _towerManager.Init();
        objetives = _objectiveList[_currentTutorial];
        _lvlCanvasManager.UpdateLevelPointBar(initialLevelPoints, initialLevelPoints, initialLevelPoints);
        _livesRemoved = 0;
        _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);

    }
    void cancelTutorial(string p)//parameter does not matter
    {
        
    }
}
