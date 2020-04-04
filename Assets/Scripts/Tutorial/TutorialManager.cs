using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public const string BLACK_OVERLAY_NAME = "tutorial_black_overlay";

    public bool enableTutorial = true;
    public AcceptPopup acceptPopup;
    /// <summary>
    /// This is the group tutorial that was before the current one
    /// </summary>
    public string lastTutorialGroupId;
    
    GameManager _gameManager;
    GenericListJsonLoader<TutorialGroup> _tutorialGroups;
    Tuple<Transform, int> _lastUIParent; //the original parent of UI element that had to be moved to be highlighted.

    public Tuple<Transform, int> LastUIParentAndSiblingIndex { get { return _lastUIParent; } }

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (!enableTutorial)
            return;

        _tutorialGroups = GameUtils.LoadConfig<GenericListJsonLoader<TutorialGroup>>("TutorialsConfig.json");
        
    }

    private void Start()
    {
        if(enableTutorial)
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
        }   
    }

    public void SetLastParentAndSibling(Transform parent, int siblingIndex)
    {
        _lastUIParent = Tuple.Create(parent, siblingIndex);
    }

    void ExecuteGroupActions(TutorialGroup g)
    {
        g.ExecuteActions();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //fucking fix for waiting All thing to get set. (Eg: main map finish loading buttons)
        StartCoroutine(OnCheckTriggers());
    }

    IEnumerator OnCheckTriggers()
    {
        yield return new WaitForFixedUpdate();
        foreach (var item in _tutorialGroups.list)
        {
            var result = item.CheckForTriggers();
            if (result)
            {
                ExecuteGroupActions(item);
                break;
            }
        }
    }
}
