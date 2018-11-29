using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{

    GameManager[] _gameManagers;
    MainMapCanvasManager _mainMapCanvas;

	void Awake () {
	}

    void Start()
    {
        _mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        _gameManagers = FindObjectsOfType<GameManager>();

        CreateLevelNodes();
    }

    void Update ()
    {
	    	
	}

    public void CreateLevelNodes()
    {
        var gm = GetRealGameManager();

        foreach (var lvlInfo in gm.LevelInfoLoader.LevelInfoList.list)
        {
			_mainMapCanvas.AddLevelButton(lvlInfo, OnLevelNodeClick, gm);
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
