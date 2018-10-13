﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public int levelID;
    public List<WalkNode> initialWalkNodes = new List<WalkNode>();
    [Tooltip("Ones camera is set, asign to this")]
    public Transform cameraTransform;

    public int initialLevelPoints;
    [Tooltip("In seconds")]
    public float levelTime = 60;
    public int pointsPerSecond = 1;
    public List<LevelSkillManager.SkillType> levelSkills = new List<LevelSkillManager.SkillType>();
    public List<Minion> availableMinions = new List<Minion>();
    [HideInInspector]
    public int[] objetives;//[5, 7, 10] first the minimun, last the maximun.
    [HideInInspector]
    public LevelMode levelMode;

    GameManager _gameManager;
    TowerManager _towerManager;
    MinionManager _minionManager;
    LevelSkillManager _lvlSkillManager;
    LevelEventManager _lvlEventManager;
    LevelGoal _levelGoal;
    LevelCanvasManager _lvlCanvasManager;
    GameObjectSelector _goSelector;
    FloorEffect _floorEffect;

    bool _levelEnded;
    int _currentLevelPoints = 0;
    float _levelTimeAux;

    /// <summary>
    /// Used to inform CurrentLevelPoints to user on the GUI.
    /// </summary>
    public int CurrentLevelPoints { get { return _currentLevelPoints; } }
    public LevelCanvasManager LevelCanvasManager { get { return _lvlCanvasManager; } }
    public MinionManager MinionManager { get { return _minionManager; } }
    public GameManager GameManager { get { return _gameManager; } }
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            SceneManager.LoadScene(0);
    }
    void Start () {
        Init();
    }

    float timerAux = 1;

	void Update ()
    {
        if(_levelEnded) return;

        GameObjectSelection();
        OnRunLevelTimer();

        if (_currentLevelPoints != initialLevelPoints)
        {
            timerAux -= Time.deltaTime;
            if (timerAux < 0)
            {
                UpdatePoints(pointsPerSecond);
                timerAux = 1;
            }
        }
    }

    void OnRunLevelTimer()
    {
        _levelTimeAux -= Time.deltaTime;
        _lvlCanvasManager.UpdateLevelTimer(_levelTimeAux < 0 ? 0 : _levelTimeAux);
        if (_levelTimeAux < 0)
        {
            _levelEnded = true;
            if(_gameManager.popupManager != null)
                _gameManager.popupManager.BuildOneButtonPopup(_lvlCanvasManager.transform, "Game Over !", "Try Again", "Main map");
            _towerManager.StopTowers();
            _minionManager.StopMinions();
        }
    }

    void GameObjectSelection()
    {
        if (_goSelector == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            var selected = _goSelector.SelectGameObject(LayerMask.NameToLayer("Minion"));
            if (selected == null) return;
            _minionManager.OnReleasedMinionSelected(selected.GetInstanceID());
        }
    }

    #region Minion Spawning Stuff

    /// <summary>
    /// Build the minion type passed in the first parameter.
    /// Returns True if minion has been created
    /// </summary>
    public bool BuildMinion(MinionType t)
    {
        if (!CheckMinionSale(t)) return false;
        var cost = _minionManager.GetMinionPrice(t);
        UpdatePoints(-cost);
        _minionManager.SpawnMinion(t, initialWalkNodes[0].transform.position
            , availableMinions.FirstOrDefault(m => m.minionType == t));
        _minionManager.SetNextMinionFree(initialWalkNodes[0]);

        return true;
    }

    bool CheckMinionSale(MinionType t)
    {
        var cost = _minionManager.GetMinionPrice(t);
        return _currentLevelPoints - cost >= 0;
    }

    public void MinionOrderHasChanged(int from, int to)
    {
        _minionManager.ChangeMinionOrder(from, to);
    }

    /// <summary>
    /// When a minion is deleted with Drag&Drop system call  this function to delete the minion itself;
    /// </summary>
    public void MinionDeletedByDandD(int index)
    {
        var coinsToAdd = _minionManager.DeleteMinionByIndex(index);
        UpdatePoints(coinsToAdd);
    }

    #endregion
    
    #region Inits()
    void Init()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if(_gameManager == null)
            SceneManager.LoadScene(0);

        var gameplayManagersGO = new GameObject("GameplayManagers");
        _minionManager = gameplayManagersGO.AddComponent<MinionManager>();
        _towerManager = gameplayManagersGO.AddComponent<TowerManager>();
        _lvlSkillManager = gameplayManagersGO.AddComponent<LevelSkillManager>();
        _goSelector = FindObjectOfType<GameObjectSelector>();
        _floorEffect = FindObjectOfType<FloorEffect>();

        _towerManager.level = _lvlSkillManager.level = _minionManager.level = this;
        
        _currentLevelPoints = initialLevelPoints;
        _levelTimeAux = levelTime;
        GameManagerInit();
        InitLevelGoal();

        InitLevelCanvas();

        ConfigureLevelEvents();


    }
    void InitLevelGoal()
    {
        _levelGoal = GetComponent<LevelGoal>();
        if (_levelGoal == null)
            throw new System.Exception("There isn't a LevelGoal object attached to this Level GO.");

        _levelGoal.OnGoalComplete += GoalCompletedHandler;
    }

    void InitLevelCanvas()
    {
        _lvlCanvasManager = FindObjectOfType<LevelCanvasManager>();

		foreach (var m in availableMinions) //need to get json data to show correct point value on spawn button 
		{
            if (m == null) {
                Debug.LogError("minion item in AvailableMinions List is null ");
                continue;
            }
			var minionStats = GameManager.MinionsLoader.GetStatByLevel (m.minionType, levelID);
			m.pointsValue = minionStats.pointsValue;
			_lvlCanvasManager.BuildAvailableMinionButton(m);
		}
        
        _lvlCanvasManager.level = this;
        _lvlCanvasManager.UpdateLevelTimer(levelTime);
        _lvlCanvasManager.UpdateLevelLives(_levelGoal.CurrentLives, _levelGoal.lives);
        UpdatePoints(0);
    }

    void GameManagerInit()
    {
        
        if (_gameManager == null)//For level only tests.
        {
        }
        else //common Behaviour
        {
            objetives = _gameManager.currentLevelInfo.objectives;
            levelMode = (LevelMode)Enum.Parse(typeof(LevelMode), _gameManager.currentLevelInfo.mode);
            levelID = _gameManager.currentLevelInfo.id;
            
        }

		_towerManager.Init ();
    }

    void ConfigureLevelEvents()
    {
        List<LevelEventManager.EventType> eventTypes = new List<LevelEventManager.EventType>();
        if (_gameManager.currentLevelInfo.weatherEvents)
        {
            _lvlEventManager = gameObject.AddComponent<LevelEventManager>();
            eventTypes.Add(LevelEventManager.EventType.Weather);
        }
        if (_gameManager.currentLevelInfo.levelEvents)
        {
            if(_lvlEventManager == null)
                _lvlEventManager = gameObject.AddComponent<LevelEventManager>();

            eventTypes.Add(LevelEventManager.EventType.Environment);
        }

        if (_lvlEventManager != null)
            _lvlEventManager.Init(eventTypes, levelID, _gameManager,this);
    }
    #endregion


    public void LoopThroughMinions(Action<Minion> action)
    {
        _minionManager.AffectMinions(action);
    }

    public void UpdateLevelGoal()
    {
        _levelGoal.UpdateGoal(-1);
        _lvlCanvasManager.UpdateLevelLives(_levelGoal.CurrentLives, _levelGoal.lives);
        _floorEffect.InitAnimation();
    }

    void GoalCompletedHandler()
    {
        Debug.Log("----- Level Completed -----");
        if(_gameManager.popupManager != null)
            _gameManager.popupManager.BuildOneButtonPopup(_lvlCanvasManager.transform, "You won!" , "Continue...", "Main map");
        _towerManager.StopTowers();
        _minionManager.StopMinions();
        _levelEnded = true;
        if (_lvlEventManager != null)
            _lvlEventManager.StopEvents();
    }
    
    public void UpdatePoints(int points)
    {
        var prevPoints = _currentLevelPoints;
        _currentLevelPoints += points;
        if (_currentLevelPoints > initialLevelPoints)
            _currentLevelPoints = initialLevelPoints;

        _lvlCanvasManager.UpdateLevelPointBar(_currentLevelPoints, prevPoints, initialLevelPoints);
    }
    
}
