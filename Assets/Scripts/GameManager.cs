using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameScenes {
        MainMap,
        Gameplay
    }

    [Header("Show DevTools.")]
    public bool showDevTools = true;
    public bool enableSwapSystem = true;
    //public CurrentScene currentScene = CurrentScene.MainMap;
    public bool enableSaveSystem;
    public PopupManager popupManager;
    public TutorialManager tutorialManager;
    public SoundManager soundManager;
    [Header("Add tower prefabs so SwapSystem can work.")]
    public List<TowerBase> allTowersPrefabs;
    public List<Minion> allMinionsPrefab;//"Add minions prefabs so can be accessed by Level.cs."
    public ParticleSystem swapParticleSystem;
    [HideInInspector]
    public bool canPressBackButton = true;

    GameScenes _currentScene;

    int _currentViewingWorld;
    string _lastSceneName;

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
    LibraryManager _libraryManager;
    LoadedAssets _loadedAssets;

    User _user;


    public LevelEventLoader LevelEventsLoader { get { return _levelEventsLoader; } }
	public MinionsJsonLoaderManager MinionsJsonLoader { get { return _minionJSONLoader; } }
	public TowerJSONLoaderManager TowerLoader { get { return _towerJSONLoader; } }
    public LevelInfo CurrentLevelInfo { get { return _currentLevelInfo; } }
    public LevelNodesLoader LevelInfoLoader { get { return _levelInfoLoader; } }
    public User User { get { return _user; } }
    public LibraryManager LibraryManager { get { return _libraryManager; } }
    public LoadedAssets LoadedAssets { get { return _loadedAssets; } }

    public int CurrentViewingWorld { get { return _currentViewingWorld; } }
    
    public string LastLoadedScene { get { return _lastSceneName; } }
    public GameScenes CurrentScene{ get { return _currentScene; } }

    public Action<bool> OnLevelInfoSet = delegate { };

    void Awake()
    {
        Application.targetFrameRate = 500;
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");

        _currentScene = GameScenes.MainMap;
        DontDestroyOnLoad(this);
        if (enableSwapSystem)
        {
            _swapTowerSystem = gameObject.AddComponent<SwapTowerSystem>();
            _swapTowerSystem.swapParticleSysPrefab = swapParticleSystem;
        }

        SaveSystem.canSave = enableSaveSystem;
    }

    private void Start()
    {
        _levelInfoLoader = new LevelNodesLoader();

        popupManager = FindObjectOfType<PopupManager>();

        _levelEventsLoader = new LevelEventLoader();
        _minionJSONLoader = new MinionsJsonLoaderManager();
		_towerJSONLoader = new TowerJSONLoaderManager ();

		_user = new User (this);
        _currentViewingWorld = _user.LevelProgressManager.GetCurrentUserWorldIndex();

        tutorialManager.Init(this);

        _libraryManager = new LibraryManager(this);
        _loadedAssets = FindObjectOfType<LoadedAssets>();

        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    public void SetCurrentLevelInfo(LevelInfo lvlinfo)
    {
        _currentLevelInfo = lvlinfo;
        _currentViewingWorld = _user.LevelProgressManager.GetCurrentUserWorldIndex();
        _currentScene = GameScenes.Gameplay;

        if (lvlinfo == null)//when comming from a level to the world selector
        {
            _currentScene = GameScenes.MainMap;
            soundManager.PlayMenuMusic();
        }
            

        OnLevelInfoSet(_currentLevelInfo != null);
    }

    public void LevelInitFinished(Level level)
    {
        if (enableSwapSystem)
            _swapTowerSystem.LeveInitFinished(level.MinionManager, level.TowerManager, level.LevelCanvasManager);

		_user.LevelStarted (level.levelID);
        level.OnLevelFinish += _user.LevelEnded;
        soundManager.PlayLevelMusic(level.levelID);
    }

    void SceneUnloaded(Scene scene)
    {
        _lastSceneName = scene.name;
    }
}
