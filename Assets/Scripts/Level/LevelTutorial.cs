using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTutorial : Level
{
    public int[] objetives1;
    public int[] objetives2;
    public int[] objetives3;

    List<int[]> _objectiveList;
    int _currentTutorial = 0;

    protected override void Init()
    {
        base.Init();
        _objectiveList = new List<int[]>(){objetives1, objetives2, objetives3};
    }

    protected override void GoalCompletedHandler()
    {
        if (_gameManager.popupManager != null)
        {
            BasePopup popup = null;
            if(_currentTutorial >= _objectiveList.Count)
                popup = _gameManager.popupManager.BuildPopup(_lvlCanvasManager.transform, "TUTORIAL COMPLETED!", "Continue tu main map", "Continue");
            else
            {
                var phaseTxt = "PHASE " + (_currentTutorial + 1) + "/" + _objectiveList.Count;
                popup = _gameManager.popupManager.BuildPopup(_lvlCanvasManager.transform
                    ,phaseTxt + " COMPLETED!"
                    , "Do you want to continue with the tutorial ?"
                    , "Continue"
                    , PopupsID.AcceptOrDecline);
            }
            
            popup.AddFunction(BasePopup.FunctionTypes.ok, setNextTutorialIfPossible);
            popup.AddFunction(BasePopup.FunctionTypes.cancel, cancelTutorial);
        }
    }

    void setNextTutorialIfPossible(string p)//parameter does not matter
    {
        _currentTutorial++;
    }
    void cancelTutorial(string p)//parameter does not matter
    {
        
    }
}
