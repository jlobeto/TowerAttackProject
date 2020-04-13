using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public const string BLACK_OVERLAY_NAME = "tutorial_black_overlay";

    public bool enableTutorial = true;
    public AcceptPopup acceptPopup;

    GameManager _gameManager;
    GenericListJsonLoader<TutorialGroup> _tutorialGroups;
    Tuple<Transform, int> _lastUIParent; //the original parent of UI element that had to be moved to be highlighted.
    /// <summary>
    /// This is the group tutorial that was before the current one
    /// </summary>
    string _lastTutorialGroupId;
    TutorialSaveDef _tutoSaveDef;

    public Tuple<Transform, int> LastUIParentAndSiblingIndex { get { return _lastUIParent; } }
    public string LastTutorialGroupId {
        get { return _lastTutorialGroupId; }
        set { _lastTutorialGroupId = value; SaveProgress(); }
    }
    public bool IsUserFirstTimeOnApp { get { return !_tutoSaveDef.didShowFirstTutoPopup; } }
    public bool UserAgreedWithMakingFirstTutorial { get { return _tutoSaveDef.didAgreeWithDoFirstTutorial; } }

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
    }

    private void Start()
    {
        if (enableTutorial)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Update()
    {

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
        _tutoSaveDef.tutorialPhasesCompleted.Add(phase.ToString());
        SaveProgress();
    }

    public bool HasUserCompletedTutorial(string phase)
    {
        return _tutoSaveDef.tutorialPhasesCompleted.Any(i => i == phase);
    }

    void ExecuteGroupActions(TutorialGroup g)
    {
        g.ExecuteActions();
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

    void CheckTriggers()
    {
        foreach (var item in _tutorialGroups.list)
        {
            var isItemCompleted = _tutoSaveDef.tutorialPhasesCompleted.Any(i => i == item.tutorialPhase);
            if (isItemCompleted) continue;

            var result = item.CheckForTriggers();
            if (result)
            {
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
