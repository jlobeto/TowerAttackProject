using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{
    public BuildSquadManager buildSquadManager;
    public GameLoadingScreen loadingJob;

    GameManager _gameManager;
    MainMapCanvasManager _mainMapCanvas;
    WorldsManager _worldsManager;

	void Awake ()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            SceneManager.LoadScene(0);
            return;
        }
            
        loadingJob.transform.parent.gameObject.SetActive(false);
        
        //_mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        //_worldsManager = new WorldsManager(GetGameManager().User);
        //CreateLevelNodes();
        //_mainMapCanvas.ShowWorld(_gameManager.CurrentViewingWorld);
    }

    void Start()
    {
        _mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        _worldsManager = new WorldsManager(GetGameManager().User);
        CreateLevelNodes();
        _mainMapCanvas.ShowWorldAtSceneInit(_gameManager.CurrentViewingWorld);
    }

    void Update ()
    {
        CheckBackButton();
    }

    
    public void CreateLevelNodes()
    {
        var gm = GetGameManager();

        var worldsUnlocked = _worldsManager.GetUnlockWorlds();

        foreach (var lvlInfo in gm.LevelInfoLoader.LevelInfoList.list)
        {
            var lvlMode = GameUtils.ToEnum(lvlInfo.mode, LevelMode.Normal);
            if (lvlMode == LevelMode.Tutorial && !gm.tutorialManager.UserAgreedWithMakingFirstTutorial)
                continue;//if user didnt accept doing tutorial, continue and don't show tutorial's node.

            bool unlocked = false;
            foreach (var worldId in worldsUnlocked)
            {
                if(worldId == lvlInfo.worldId)
                {
                    unlocked = true;
                    break;
                }
            }
			if (lvlInfo.worldId == 0) 
			{
				_mainMapCanvas.AddLevelButton (lvlInfo, OnLevelNodeClick, gm, true, 0);	
				continue;
			}

            var starsLeft = _worldsManager.GetStarsLeftAmount(lvlInfo.worldId);
            var allLevelsWon = gm.User.LevelProgressManager.AreLevelsWonByWorld(lvlInfo.worldId-1);

			_mainMapCanvas.AddLevelButton (lvlInfo, OnLevelNodeClick, gm, unlocked /*&& allLevelsWon*/, starsLeft);	
        }
    }

    void OnLevelNodeClick(LevelInfo lvlInfo)
    {
		GetGameManager().SetCurrentLevelInfo(lvlInfo);
        SoundManager.instance.PlaySound(SoundFxNames.button_pressed);

        buildSquadManager.DisplayPopup();
        buildSquadManager.OnPlayPressed += OnAcceptSquad;
    }

    void OnAcceptSquad()
    {
        var lvlInfo = GetGameManager().CurrentLevelInfo;
        buildSquadManager.OnPlayPressed -= OnAcceptSquad;

        try
        {
            _mainMapCanvas.gameObject.SetActive(false);
            loadingJob.ActivateLoadingAsyncProcess("Level" + lvlInfo.id);
        }
        catch (System.Exception)
        {
            throw new System.Exception("Error Loading LevelScene > There is not a scene called 'Level" + lvlInfo.id + "'");
        }
    }

	public GameManager GetGameManager()
	{
		return _gameManager;
	}

    void CheckBackButton()
    {
        if(GetGameManager().canPressBackButton && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScreen");
        }
    }
}
