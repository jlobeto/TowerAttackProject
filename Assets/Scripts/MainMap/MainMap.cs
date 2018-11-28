using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{

    GameManager[] _gameManagers;
    LevelNodesLoader _levelInfoLoader;
    MainMapCanvasManager _mainMapCanvas;

	void Awake () {
        _levelInfoLoader = new LevelNodesLoader();
	}

    void Start()
    {
        _mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        _gameManagers = FindObjectsOfType<GameManager>();

        CreateLevelNodes();
    }

    void Update () {
		
	}

    void CreateLevelNodes()
    {
        foreach (var lvlInfo in _levelInfoLoader.LevelInfoList.list)
        {
            _mainMapCanvas.AddLevelButton(lvlInfo, OnLevelNodeClick);
        }
    }

    void OnLevelNodeClick(LevelInfo lvlInfo)
    {

        for (int i = 0; i < _gameManagers.Length; i++)
        {
            if (_gameManagers[i] == null) continue;
            _gameManagers[i].SetCurrentLevelInfo(lvlInfo);
        }

        try
        {
            SceneManager.LoadScene("Level" + lvlInfo.id);
        }
        catch (System.Exception)
        {
            throw new System.Exception("Error Loading LevelScene > There is not a scene called 'Level"+lvlInfo.id+"'");
        }
        
    }
}
