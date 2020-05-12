using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public const string BLACK_OVERLAY_NAME = "tutorial_black_overlay";
    public const string POINTING_FINGER_NAME = "pointing_finger_image";
    public const string TUTORIAL_TEXT_NAME = "tutorial_text";

    public bool enableTutorial = true;
    public AcceptPopup acceptPopup;
    public Image pointingFinger;
    public Font texts_font;
    public Action OnForceExecutingOutputFinished = delegate { };
    public Action OnScreenTouched = delegate { };
    [HideInInspector]
    public bool canCheckUpdateForScreeenTouch;
    [HideInInspector]
    public Text tutorialText;//Used in tutorials. With this i can have a reference.

    GameManager _gameManager;
    GenericListJsonLoader<TutorialGroup> _tutorialGroups;
    Tuple<Transform, int> _lastUIParent; //the original parent of UI element that had to be moved to be highlighted.
    /// <summary>
    /// This is the group tutorial that was before the current one
    /// </summary>
    string _lastTutorialGroupId;
    TutorialSaveDef _tutoSaveDef;
    bool _isTutorialGroupRunning;

    public Tuple<Transform, int> LastUIParentAndSiblingIndex { get { return _lastUIParent; } }
    public string LastTutorialGroupId {
        get { return _lastTutorialGroupId; }
        set {
            _lastTutorialGroupId = value;
            _tutoSaveDef.lastTutorialGroupId = _lastTutorialGroupId;
            SaveProgress();
        }
    }
    public bool IsUserFirstTimeOnApp { get { return !_tutoSaveDef.didShowFirstTutoPopup; } }
    public bool UserAgreedWithMakingFirstTutorial { get { return _tutoSaveDef.didAgreeWithDoFirstTutorial; } }
    /// <summary>
    /// //Is the timer of one group output(StartNextTutorialGroupOnTimer) finished so the next group tuto can be triggered? 
    /// </summary>
    public bool TimerForNextGroupTutorialIsFinished { get { return _timerForNextGroupTutorialIsFinished; } }

    bool _timerForNextGroupTutorialIsFinished = true;

    List<string> tutorialGroupsDone;
    void Awake()
    {
        DontDestroyOnLoad(this);

        _tutorialGroups = GameUtils.LoadConfig<GenericListJsonLoader<TutorialGroup>>("TutorialsConfig.json");
        _tutoSaveDef = SaveSystem.Load<TutorialSaveDef>(SaveSystem.TUTORIAL_SAVE_NAME);
        if (_tutoSaveDef == null)
        {
            _tutoSaveDef = new TutorialSaveDef();
            SaveSystem.Save(_tutoSaveDef, SaveSystem.TUTORIAL_SAVE_NAME);
        }

        tutorialGroupsDone = new List<string>();
    }

    private void Start()
    {
        if (enableTutorial)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {
        if(canCheckUpdateForScreeenTouch)
        {
            if(Input.GetMouseButtonDown(0))
            {
                OnScreenTouched();
            }
        }
    }

    public void Init(GameManager gm)
    {
        if (!enableTutorial) return;

        _gameManager = gm;
        foreach (var item in _tutorialGroups.list)
        {
            item.SetTutorialGroup(this, _gameManager);
            item.OnTutorialFinished += CheckTriggers;
        }
    }

    public void SetLastParentAndSibling(Transform parent, int siblingIndex)
    {
        _lastUIParent = Tuple.Create(parent, siblingIndex);
    }

    public void FirstTimeAppIsOpened()
    {
        _tutoSaveDef.didShowFirstTutoPopup = true;
        SaveProgress();
    }

    public void UserAgreesWithDoFirstTutorial(bool didAgree)
    {
        _tutoSaveDef.didAgreeWithDoFirstTutorial = didAgree;
        SaveProgress();
    }

    public void TutorialFinished(TutorialPhase phase)
    {
        _isTutorialGroupRunning = false;
        _tutoSaveDef.tutorialPhasesCompleted.Add(phase.ToString());
        SaveProgress();
    }

    public bool HasUserCompletedTutorial(string phase)
    {
        return _tutoSaveDef.tutorialPhasesCompleted.Any(i => i == phase);
    }

    public void StartNextTutorialGroupOnTimer(float seconds)
    {
        _timerForNextGroupTutorialIsFinished = false;
        StartCoroutine(StartNewTutorialGroup(seconds));
    }

    IEnumerator StartNewTutorialGroup(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        _timerForNextGroupTutorialIsFinished = true;
        CheckTriggers();
    }

    public void ForceExecutingOutputAfterSeconds(float sec)
    {
        StartCoroutine(StartForceExecutingOutputAfterSeconds(sec));
    }
    IEnumerator StartForceExecutingOutputAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        OnForceExecutingOutputFinished();
    }

    void ExecuteGroupActions(TutorialGroup g)
    {
        g.ExecuteActions();
        tutorialGroupsDone.Add(g.tutorialGroupId);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //fucking fix for waiting All thing to get set. (Eg: main map finish loading buttons)
        StartCoroutine(OnCheckTriggersCoroutine());
    }

    IEnumerator OnCheckTriggersCoroutine()
    {
        yield return new WaitForFixedUpdate();
        CheckTriggers();
    }

    public void CheckTriggers()
    {
        foreach (var item in _tutorialGroups.list)
        {

            var isItemCompleted = _tutoSaveDef.tutorialPhasesCompleted.Any(i => i == item.tutorialPhase);
            if (isItemCompleted) continue;

            var tutorialGroupHasBeenDone = tutorialGroupsDone.Any(i => i == item.tutorialGroupId);
            if (tutorialGroupHasBeenDone) continue;

            var result = item.CheckForTriggers();
            if (result)
            {
                _isTutorialGroupRunning = true;
                ExecuteGroupActions(item);
                break;
            }
        }
    }

    void SaveProgress()
    {
        SaveSystem.Save(_tutoSaveDef, SaveSystem.TUTORIAL_SAVE_NAME);
    }
}
