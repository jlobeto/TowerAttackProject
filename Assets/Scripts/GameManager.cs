using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum CurrentScene {
        MainMap,
        Gameplay
    }

    [Header("Show DevTools.")]
    public bool showDevTools = true;

    //public CurrentScene currentScene = CurrentScene.MainMap;
    public PopupManager popupManager;
    public TutorialManager tutorialManager;
    [Header("Add tower prefabs so SwapSystem can work.")]
    public List<TowerBase> allTowersPrefabs;
    public List<Minion> allMinionsPrefab;//"Add minions prefabs so can be accessed by Level.cs."
    public ParticleSystem swapParticleSystem;

    int _currentViewingWorld;

    /// <summary>
    /// This is set when a level node is clicked, When returning from a level, this is set to null.
    /// Using this to set some variables to the Level Object on Level Scene
    /// </summary>
    LevelInfo _currentLevelInfo;

    LevelEventLoader _levelEventsLoader;
	MinionsJsonLoaderManager _minionJSONLoader;
	TowerJSONLoaderManager _towerJSONLoader;
    SwapTowerSystem _swapTowerSystem;
    LevelNodesLoader _levelInfoLoader;
    

    User _user;

    public LevelEventLoader LevelEventsLoader { get { return _levelEventsLoader; } }
	public MinionsJsonLoaderManager MinionsJsonLoader { get { return _minionJSONLoader; } }
	public TowerJSONLoaderManager TowerLoader { get { return _towerJSONLoader; } }
    public LevelInfo CurrentLevelInfo { get { return _currentLevelInfo; } }
    public LevelNodesLoader LevelInfoLoader { get { return _levelInfoLoader; } }
    public User User { get { return _user; } }

    public int CurrentViewingWorld { get { return _currentViewingWorld; } }

    public Action<bool> OnLevelInfoSet = delegate { };

    void Awake()
    {
        DontDestroyOnLoad(this);
        _swapTowerSystem = gameObject.AddComponent<SwapTowerSystem>();
        _swapTowerSystem.swapParticleSysPrefab = swapParticleSystem;

    }

    private void Start()
    {
        _levelInfoLoader = new LevelNodesLoader();

        popupManager = FindObjectOfType<PopupManager>();

        _levelEventsLoader = new LevelEventLoader();
        _minionJSONLoader = new MinionsJsonLoaderManager();
		_towerJSONLoader = new TowerJSONLoaderManager ();

		_user = new User (this);

        tutorialManager.Init(this);
    }

    public void SetCurrentLevelInfo(LevelInfo lvlinfo)
    {
        _currentLevelInfo = lvlinfo;

        if (lvlinfo != null)
            _currentViewingWorld = lvlinfo.worldId;

        OnLevelInfoSet(_currentLevelInfo != null);
    }

    public void LevelInitFinished(Level level)
    {
		_swapTowerSystem.LeveInitFinished(level.MinionManager, level.TowerManager, level.LevelCanvasManager);

		_user.LevelStarted (level.levelID);
        level.OnLevelFinish += _user.LevelEnded;
    }
}
