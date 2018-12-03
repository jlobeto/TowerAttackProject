using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{

    GameManager[] _gameManagers;
    MainMapCanvasManager _mainMapCanvas;
    WorldsManager _worldsManager;
	void Awake ()
    {

	}

    void Start()
    {
        _mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        _gameManagers = FindObjectsOfType<GameManager>();

        _worldsManager = new WorldsManager(GetRealGameManager().User);
        

        CreateLevelNodes();

    }

    void Update ()
    {
        
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
            var starsLeft = _worldsManager.GetStarsLeftAmount(lvlInfo.worldId);
			_mainMapCanvas.AddLevelButton(lvlInfo, OnLevelNodeClick, gm, unlocked, starsLeft);
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
		for (int i = 0; i < _gameManagers.Length; i++)
		{
			if (_gameManagers[i] == null ) continue;

            //Have to do this to check if it is the correct gamemanager.
            //Look for something on the internet to not do that and only create one gamemanager (that use dont destroy onload)
            if (_gameManagers[i].User == null)
            {
                Destroy(_gameManagers[i].gameObject);
                continue;
            }

			return _gameManagers [i];
		}

		return null;
	}
}
