using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public const string BLACK_OVERLAY_NAME = "tutorial_black_overlay";

    public bool enableTutorial = true;
    public AcceptPopup acceptPopup;
    
    GameManager _gameManager;
    GenericListJsonLoader<TutorialGroup> _tutorialGroups;


    void Awake()
    {
        DontDestroyOnLoad(this);

        if (!enableTutorial)
            return;

        _tutorialGroups = GameUtils.LoadConfig<GenericListJsonLoader<TutorialGroup>>("TutorialsConfig.json");
        foreach (var item in _tutorialGroups.list)
        {
            item.SetTutorialGroup(this);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void Update()
    {
        
    }

    public void Init(GameManager gm)
    {
        _gameManager = gm;
        
    }

    void ExecuteGroupActions(TutorialGroup g)
    {
        g.ExecuteActions();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
