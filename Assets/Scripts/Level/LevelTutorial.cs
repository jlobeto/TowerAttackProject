using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTutorial : Level
{
    public int[] objetives1;
    public int[] objetives2;
    public int[] objetives3;
    public GameObject towersPhase2;
    public GameObject towersPhase3;
    public Sprite pressingFingerSprite;
    public Transform phase3FingerAnimEndPos;

    public List<Image> fingersPhase2;
    [HideInInspector]
    public bool phase3MinionSkillSelected;

    List<int[]> _objectiveList;
    Canvas _lvlCanvas;
    int _currentTutorial = 0;
    public int CurrentTutorialPart { get { return _currentTutorial; } }
    Camera _uiCam;
    Camera _mainCam;
    int _uiCameraOldCulling;
    int _mainCameraOldCulling;
    bool _initWithFirstPhase = true;//when starting level-1 from another phase(not 1) turn this to false because Level is executed before OnCheckTriggersCoroutine;
    bool _initFromPhase2 = false;

    protected override void Init()
    {
        base.Init();

        foreach (var item in fingersPhase2)
        {
            item.gameObject.SetActive(false);
        }

        _lvlCanvas = _lvlCanvasManager.GetComponent<Canvas>();
        _lvlCanvasManager.EnableDisableMinionSkillButtons(false);

        _objectiveList = new List<int[]>(){objetives1, objetives2, objetives3};

        var cams = FindObjectsOfType<Camera>();
        _mainCam = cams.FirstOrDefault(i => i.tag == "MainCamera");
        _mainCameraOldCulling = _mainCam.cullingMask;
        _uiCam = cams.FirstOrDefault(i => i.clearFlags == CameraClearFlags.Depth);
        _uiCameraOldCulling = _uiCam.cullingMask;

        if (_gameManager.tutorialManager.HasUserCompletedTutorial(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase1.ToString()))
        {
            if (_gameManager.tutorialManager.HasUserCompletedTutorial(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase2.ToString()))
                _currentTutorial = 2;//phase3
            else
            {
                _currentTutorial = 1;//phase2
                _initFromPhase2 = true;
            }

            _initWithFirstPhase = false;
            setNextTutorialIfPossible("");
        }
        else
        {
            objetives = _objectiveList[_currentTutorial];
            _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);
        }
    }

    protected override void GoalCompletedHandler(bool shouldShowPopup = true)
    {
        if (_gameManager.popupManager != null)
        {
            _minionManager.StopMinions();
            _towerManager.StopOrInitTowers();

            _lvlCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            BasePopup popup = null;
            if(_currentTutorial+1 >= _objectiveList.Count)
            {
                _gameManager.tutorialManager.TutorialFinished(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase3);
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
                    , "Exit"
                    , PopupsID.AcceptOrDecline);

                popup.transform.localScale *= 1.5f;
                popup.AddFunction(BasePopup.FunctionTypes.ok, setNextTutorialIfPossible);
                popup.AddFunction(BasePopup.FunctionTypes.cancel, cancelTutorial);

                _currentTutorial++;
            }

            popup.transform.localScale = new Vector3(.7f, .7f, .7f);
        }
    }

    void setNextTutorialIfPossible(string p)//parameter does not matter
    {
        _lvlCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _uiCam.cullingMask = (1 << LayerMask.NameToLayer("Tower") | 1 << LayerMask.NameToLayer("Minion"));
        _mainCam.cullingMask = ~(1 << LayerMask.NameToLayer("Minion") | 1 << LayerMask.NameToLayer("Tower"));//render everything except for minion and tower

        if (_currentTutorial == 1)//phase 2
        {
            towersPhase2.SetActive(true);
            _gameManager.tutorialManager.TutorialFinished(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase1);
        }
        else
        {
            _gameManager.tutorialManager.TutorialFinished(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase2);
            towersPhase3.SetActive(true);
            towersPhase2.SetActive(false);
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

        if(_initWithFirstPhase || _initFromPhase2)
            _gameManager.tutorialManager.CheckTriggers();
    }

    void cancelTutorial(string p)//parameter does not matter
    {
        _gameManager.tutorialManager.TutorialFinished(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase1);
        _gameManager.tutorialManager.TutorialFinished(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase2);
        _gameManager.tutorialManager.TutorialFinished(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase3);

        OnFinishLevelCallback("");
    }
    

    public void InitFingerAnimation(bool setFinger)
    {
        if(setFinger)
        {
            var finger = FindObjectOfType<TutorialFingerAnimation>();
            if (finger == null) return;

            finger.GetComponent<Image>().sprite = pressingFingerSprite;

            finger.transform.SetParent(_lvlCanvas.transform);
            finger.InitAnimation(finger.transform, phase3FingerAnimEndPos);
        }

        _minionManager.OnMinionSkillSelected += OnMinionSkillSelected;
    }

    public void SetMinionsAndTowers(bool enabled)
    {
        
        _towerManager.StopOrInitTowers(enabled);
        _minionManager.AffectMinions((m) => m.SetWalk(enabled));

    }

    void OnMinionSkillSelected(MinionType type)
    {
        _minionManager.OnMinionSkillSelected -= OnMinionSkillSelected;
        phase3MinionSkillSelected = true;
        _gameManager.tutorialManager.CheckTriggers();
    }
}
