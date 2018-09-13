using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{

    GameManager _gameManager;
    LevelNodesLoader _levelInfoLoader;
    MainMapCanvasManager _mainMapCanvas;

	void Awake () {
        _levelInfoLoader = new LevelNodesLoader();
	}

    void Start()
    {
        _mainMapCanvas = FindObjectOfType<MainMapCanvasManager>();
        _gameManager = FindObjectOfType<GameManager>();
        CreateLevelNodes();
    }

    void Update () {
		
	}

    void CreateLevelNodes()
    {
        foreach (var lvlInfo in _levelInfoLoader.LevelInfoList.levelsInfo)
        {
            _mainMapCanvas.AddLevelButton(lvlInfo, OnLevelNodeClick);
        }
    }

    void OnLevelNodeClick(LevelInfo lvlInfo)
    {
        _gameManager.currentLevelInfo = lvlInfo;

        try
        {
            SceneManager.LoadScene("Level" + lvlInfo.id);
        }
        catch (System.Exception)
        {
            throw new System.Exception("Error Loading LevelScene > There is not a scene called Level"+lvlInfo.id);
        }
        
    }
}
