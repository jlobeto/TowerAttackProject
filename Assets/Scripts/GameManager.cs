using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CurrentScene {
        MainMap,
        Gameplay
    }

    /// <summary>
    /// This is set when a level node is clicked, When returning from a level, this is set to null.
    /// Using this to set some variables to the Level Object on Level Scene
    /// </summary>
    public LevelInfo currentLevelInfo;
    public CurrentScene currentScene = CurrentScene.MainMap;
    public PopupManager popupManager;

    LevelEventLoader _levelEventsLoader;
	MinionsJsonLoaderManager _minionJSONLoader;
	TowerJSONLoaderManager _towerJSONLoader;

    public LevelEventLoader LevelEventsLoader { get { return _levelEventsLoader; } }
	public MinionsJsonLoaderManager MinionsLoader { get { return _minionJSONLoader; } }
	public TowerJSONLoaderManager TowerLoader { get { return _towerJSONLoader; } }


    void Awake()
    {
        DontDestroyOnLoad(this);
	}

    private void Start()
    {
        popupManager = FindObjectOfType<PopupManager>();

        _levelEventsLoader = new LevelEventLoader();
        _minionJSONLoader = new MinionsJsonLoaderManager();
		_towerJSONLoader = new TowerJSONLoaderManager ();
    }
}
