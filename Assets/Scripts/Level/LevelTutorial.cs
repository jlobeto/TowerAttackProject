using System.Linq;
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
    Canvas _lvlCanvas;
    int _currentTutorial = 0;
    public int CurrentTutorialPart { get { return _currentTutorial; } }
    Camera _uiCam;
    Camera _mainCam;
    int _uiCameraOldCulling;
    int _mainCameraOldCulling;

    protected override void Init()
    {
        base.Init();
        _objectiveList = new List<int[]>(){objetives1, objetives2, objetives3};

        objetives = _objectiveList[_currentTutorial];
        _lvlCanvasManager.EnableDisableMinionSkillButtons(false);
        _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);

        _lvlCanvas = _lvlCanvasManager.GetComponent<Canvas>();
        var cams = FindObjectsOfType<Camera>();
        _mainCam = cams.FirstOrDefault(i => i.tag == "MainCamera");
        _mainCameraOldCulling = _mainCam.cullingMask;
        _uiCam = cams.FirstOrDefault(i => i.clearFlags == CameraClearFlags.Depth);
        _uiCameraOldCulling = _uiCam.cullingMask;
    }

    protected override void GoalCompletedHandler(bool shouldShowPopup = true)
    {
        if (_gameManager.popupManager != null)
        {
            _minionManager.StopMinions();
            
            _lvlCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

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
        _lvlCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _currentTutorial++;
        if (_currentTutorial == 1)//phase 2
        {
            _towersPhase2.SetActive(true);
            _uiCam.cullingMask = (1 << LayerMask.NameToLayer("Tower") | 1 << LayerMask.NameToLayer("Minion"));
            _mainCam.cullingMask = ~(1 << LayerMask.NameToLayer("Minion") | 1 << LayerMask.NameToLayer("Tower"));//render everything except for minion and tower
        }
        else
        {
            //_uiCam.cullingMask = _uiCameraOldCulling;
            //_mainCam.cullingMask = _mainCameraOldCulling;
            _towersPhase3.SetActive(true);
            _towersPhase2.SetActive(false);
            _lvlCanvasManager.EnableDisableMinionSkillButtons(true);
        }

        _towerManager.Init();
        _minionManager.DeleteAllMinionsInPath();

        var minionsInfos = GameObject.Find("InfoCanvasParent");
        if (minionsInfos != null) Destroy(minionsInfos);

        objetives = _objectiveList[_currentTutorial];
        _lvlCanvasManager.UpdateLevelPointBar(initialLevelPoints, initialLevelPoints, initialLevelPoints);
        _livesRemoved = 0;
        _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);

    }
    void cancelTutorial(string p)//parameter does not matter
    {
        
    }
}
