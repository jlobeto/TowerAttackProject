using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{

    GameManager _gameManager;
    MainMapCanvasManager _mainMapCanvas;
    WorldsManager _worldsManager;
	void Awake ()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            SceneManager.LoadScene(0);
    }

    void Start()
    {
        _mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        

        _worldsManager = new WorldsManager(GetRealGameManager().User);
        

        CreateLevelNodes();

    }

    void Update ()
    {
        CheckBackButton();
    }




    public void CreateLevelNodes()
    {
        var gm = GetRealGameManager();

        var worldsUnlocked = _worldsManager.GetUnlockWorlds();

        foreach (var lvlInfo in gm.LevelInfoLoader.LevelInfoList.list)
        {
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

			_mainMapCanvas.AddLevelButton (lvlInfo, OnLevelNodeClick, gm, unlocked && allLevelsWon, starsLeft);	
        }
    }

    void OnLevelNodeClick(LevelInfo lvlInfo)
    {
		GetRealGameManager().SetCurrentLevelInfo(lvlInfo);

        try
        {
            SceneManager.LoadScene("Level" + lvlInfo.id);
        }
        catch (System.Exception)
        {
            throw new System.Exception("Error Loading LevelScene > There is not a scene called 'Level"+lvlInfo.id+"'");
        }
        
    }

	public GameManager GetRealGameManager()
	{
		return _gameManager;
	}

    void CheckBackButton()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScreen");
        }
    }
}
