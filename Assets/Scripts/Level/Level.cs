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

    [HideInInspector] public int initialLevelPoints;
    [HideInInspector] public float levelTime = 60;
    public int pointsPerSecond = 1;
    //public List<LevelSkillManager.SkillType> levelSkills = new List<LevelSkillManager.SkillType>();
    public List<Minion> availableMinions = new List<Minion>();
    [HideInInspector]
    public int[] objetives;//[5, 7, 10] first the minimun, last the maximun.
    [HideInInspector]
    public int[] currencyWinPerObjetives;

    public LevelMode levelMode;
	public Action<int, bool, int> OnLevelFinish = delegate {}; //<lvlid, win ?, stars> - User.LevelEnded();
    public LevelPortalEffect levelPortal;
    public MinionInOutElectricity rayPathMinionIn;
    public MinionInOutElectricity rayPathMinionOut;

    protected GameManager _gameManager;
    protected TowerManager _towerManager;
    protected MinionManager _minionManager;
	protected MinionsSkillManager _minionSkillManager;
    protected LevelSkillManager _lvlSkillManager;
    protected LevelEventManager _lvlEventManager;
    protected LevelCanvasManager _lvlCanvasManager;
    protected GameObjectSelector _goSelector;
    protected FloorEffect _floorEffect;
    protected int _livesRemoved;
    protected bool _levelEnded;

    int _currentLevelPoints = 0;
    float _levelTimeAux;
    

    /// <summary>
    /// Used to inform CurrentLevelPoints to user on the GUI.
    /// </summary>
    public int CurrentLevelPoints { get { return _currentLevelPoints; } }
    public int LivesRemoved { get { return _livesRemoved; } }
    public LevelCanvasManager LevelCanvasManager { get { return _lvlCanvasManager; } }
    public MinionManager MinionManager { get { return _minionManager; } }
    public TowerManager TowerManager { get { return _towerManager; } }
	public GameManager GameManager { get { return _gameManager; } }
	public MinionsSkillManager MinionSkillManager { get { return _minionSkillManager; } }
	public GameObjectSelector GameObjectSelector { get { return _goSelector; } }

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

	protected virtual void Update ()
    {
        if(_levelEnded) return;

        GameObjectSelection();
        OnRunLevelTimer();
		CheckBackButton ();

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

	void CheckBackButton()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
            if (levelMode == LevelMode.Tutorial) return;

            var popup = GameManager.popupManager.BuildPopup(LevelCanvasManager.transform, "Pause", "Game paused", "Continue", "Main Map", PopupsID.AcceptOrDecline, true);
            if (popup != null)
            {
                Time.timeScale = 0;//TODO:: // SACAR ESTO A LA MIERDA
                _lvlCanvasManager.EnableMinionButtons(false);
                _lvlCanvasManager.EnableDisableMinionSkillButtons(false);

                popup.AddFunction(BasePopup.FunctionTypes.ok, OnResumePause);
                popup.AddFunction(BasePopup.FunctionTypes.cancel, OnFinishLevelCallback);
                popup.transform.localScale *= 1.7f;
            }
        }
	}
    void OnResumePause(string p)
    {
        Time.timeScale = 1;//TODO:: // SACAR ESTO A LA MIERDA
        _lvlCanvasManager.EnableMinionButtons(true);
        _lvlCanvasManager.EnableDisableMinionSkillButtons(true);
        GameManager.popupManager.DisplayedPopupWasClosed();
    }

    void OnRunLevelTimer()
    {
		if (levelMode == LevelMode.Tutorial)
            return;
			
		
        _levelTimeAux -= Time.deltaTime;
        _lvlCanvasManager.UpdateLevelTimer(_levelTimeAux < 0 ? 0 : _levelTimeAux, levelTime);
        if (_levelTimeAux < 0)
        {
            _levelEnded = true;
            CheckLevelCompletion(true);
            _towerManager.StopOrInitTowers();
            _minionManager.StopMinions();
        }
    }

    void GameObjectSelection()
    {
        if (_goSelector == null) return;

        if (Input.GetMouseButtonDown(0))
        {
			var selected = _goSelector.SelectGameObject(LayerMask.NameToLayer("Tower"));
			if(selected != null)
			{
				_towerManager.ActivateTowerAttackRange(selected.GetInstanceID());
			}
        }
    }

    #region Minion Spawning Stuff

    /// <summary>
    /// Build the minion type passed in the first parameter.
    /// Returns True if minion has been created
    /// </summary>
    public virtual bool BuildMinion(MinionType t)
    {
        if (!CheckMinionSale(t)) return false;
        var cost = _minionManager.GetMinionPrice(t);
        UpdatePoints(-cost);
        _minionManager.SpawnMinion(t, GetInitWalkNode().transform.position
            , availableMinions.FirstOrDefault(m => m.minionType == t));
        _minionManager.SetNextMinionFree(GetInitWalkNode());

        if (rayPathMinionIn != null)
            rayPathMinionIn.InitElectricity();

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
    protected virtual void Init()
    {
        var gameplayManagersGO = new GameObject("GameplayManagers");
        _minionManager = gameplayManagersGO.AddComponent<MinionManager>();
        _towerManager = gameplayManagersGO.AddComponent<TowerManager>();
        //_lvlSkillManager = gameplayManagersGO.AddComponent<LevelSkillManager>();
		_minionSkillManager = gameplayManagersGO.AddComponent<MinionsSkillManager>();
        _goSelector = FindObjectOfType<GameObjectSelector>();
        _floorEffect = FindObjectOfType<FloorEffect>();

		//_lvlSkillManager.level = this;
		_towerManager.level = _minionManager.level = this;

        BuildAvailableMinions();

        SetGameManagerData();

        InitLevelCanvas();

        ConfigureLevelEvents();

        _gameManager.LevelInitFinished(this);
		_minionSkillManager.Init (this);

        InitRayPathMinionInOut();
    }

    

    void BuildAvailableMinions()
    {
        var squadOrder = _gameManager.User.GetSquadMinionsOrder();
        var prefabMinions = _gameManager.allMinionsPrefab;
        foreach (var item in squadOrder)
        {
            foreach (var prefab in prefabMinions)
            {
                if (prefab.minionType.ToString() == item)
                    availableMinions.Add(prefab);
            }
        }
    }
    
    protected virtual void InitLevelCanvas()
    {
        if(_lvlCanvasManager == null)
            _lvlCanvasManager = FindObjectOfType<LevelCanvasManager>();

		_lvlCanvasManager.level = this;
        
        _lvlCanvasManager.BuildMinionSlots (availableMinions, levelID, _minionSkillManager);
        _lvlCanvasManager.UpdateLevelTimer(levelTime, levelTime, levelMode == LevelMode.Tutorial);

        _lvlCanvasManager.SetLivesUI();
        _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length-1]);
        UpdatePoints(0);
    }

    void SetGameManagerData()
    {
        objetives = _gameManager.CurrentLevelInfo.objectives;
        initialLevelPoints = _gameManager.CurrentLevelInfo.initialLevelPoints;
        _currentLevelPoints = initialLevelPoints;
        levelTime = _gameManager.CurrentLevelInfo.levelTime;
        _levelTimeAux = levelTime;
        currencyWinPerObjetives = _gameManager.CurrentLevelInfo.currencyGainedByObjectives;
        levelMode = (LevelMode)Enum.Parse(typeof(LevelMode), _gameManager.CurrentLevelInfo.mode);
        levelID = _gameManager.CurrentLevelInfo.id;
        _towerManager.Init ();
    }

    void ConfigureLevelEvents()
    {
        List<LevelEventManager.EventType> eventTypes = new List<LevelEventManager.EventType>();
        if (_gameManager.CurrentLevelInfo.weatherEvents)
        {
            _lvlEventManager = gameObject.AddComponent<LevelEventManager>();
            eventTypes.Add(LevelEventManager.EventType.Weather);
        }
        if (_gameManager.CurrentLevelInfo.levelEvents)
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
        _livesRemoved++;
        _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);
        _floorEffect.InitAnimation();

        if (levelPortal != null)
            levelPortal.UpdateGoal(LivesRemoved, objetives);

        if(rayPathMinionOut != null)
            rayPathMinionOut.InitElectricity();

        CheckLevelCompletion(false);
    }

    void CheckLevelCompletion(bool byLevelTimer)
    {
        if (LivesRemoved == objetives[objetives.Length - 1])
        {
            GoalCompletedHandler();
        }
        else if (byLevelTimer)
        {
            if (LivesRemoved >= objetives[0])
            {
                GoalCompletedHandler();
            }
            else if (LivesRemoved < objetives[0])
            {
                _lvlCanvasManager.EnableMinionButtons(false);
                _lvlCanvasManager.EnableDisableMinionSkillButtons(false);

                OnLevelFinish (levelID, false, GetCurrentStarsWinning());

                if (_gameManager.popupManager != null)
                {
                    var popup = _gameManager.popupManager.BuildPopup(_lvlCanvasManager.transform, "", "", "Retry", "Main Map", PopupsID.LevelEndPopup);
                    if (popup != null)
                    {
                        popup.AddFunction(BasePopup.FunctionTypes.close, OnFinishLevelCallback);
                        popup.InitPopup("false,," + levelID + ",,");
                    }
                        

                }

            }
        }
    }

    /// <summary>
    /// For testing
    /// </summary>
    public virtual void ForceLevelWin(int stars)
    {
        _livesRemoved = objetives[stars-1];
        GoalCompletedHandler();
    }

    protected virtual void GoalCompletedHandler(bool shouldShowPopup = true)
    {
        //Debug.Log("----- Level Completed -----");
        if(_gameManager.popupManager != null && shouldShowPopup)
        {       
            var popup = _gameManager.popupManager.BuildPopup(_lvlCanvasManager.transform,"", "", "Continue", "Main Map", "Retry", PopupsID.LevelEndPopup);
            popup.AddFunction(BasePopup.FunctionTypes.ok, OnFinishLevelCallback);
            popup.InitPopup("true,"+ GetCurrentStarsWinning() + "," + levelID + ","+LivesRemoved+","+ _gameManager.User.GetCoinsGained(levelID, GetCurrentStarsWinning()));
            popup.transform.localScale *= 1.8f;
        }

        _towerManager.StopOrInitTowers();
        _minionManager.StopMinions();
        _levelEnded = true;
        if (_lvlEventManager != null)
            _lvlEventManager.StopEvents();

        _lvlCanvasManager.EnableMinionButtons(false);
        _lvlCanvasManager.EnableDisableMinionSkillButtons(false);
        OnLevelFinish (levelID, true, GetCurrentStarsWinning());
    }
    
    public void UpdatePoints(int points)
    {
        var prevPoints = _currentLevelPoints;
        _currentLevelPoints += points;
        if (_currentLevelPoints > initialLevelPoints)
            _currentLevelPoints = initialLevelPoints;

        _lvlCanvasManager.UpdateLevelPointBar(_currentLevelPoints, prevPoints, initialLevelPoints);
    }
    

	public int GetCurrentStarsWinning()
	{
        return GameplayUtils.StarsWon(LivesRemoved, objetives);
	}

    public WalkNode GetInitWalkNode()
    {
        if (initialWalkNodes[0] != null)
            return initialWalkNodes[0];

        var walkNodes = FindObjectsOfType<WalkNode>();
        foreach (var item in walkNodes)
        {
            if(item.isInitial)
            {
                initialWalkNodes[0] = item;
                return item;
            }
        }

        return null;
    }

    WalkNode GetEndWalkNode()
    {
        var walkNodes = FindObjectsOfType<WalkNode>();

        foreach (var item in walkNodes)
        {
            if (item.isEnd)
                return item;
        }
        Debug.LogError("There is not a walk node set as END");
        return null;
    }

    void InitRayPathMinionInOut()
    {
        var initWalkNode = GetInitWalkNode();
        var endWalkNode = GetEndWalkNode();

        if (rayPathMinionIn != null)
        {
            rayPathMinionIn.transform.position = initWalkNode.transform.position;
            rayPathMinionIn.Init(initWalkNode);
        }
        else
        {
            var go = GameObject.FindWithTag("ray_path_minion_in");
            if (go == null)
                Debug.LogError("ray path minion in is null!!!");
            else
            {
                rayPathMinionIn = go.GetComponent<MinionInOutElectricity>();
                rayPathMinionIn.transform.position = initWalkNode.transform.position;
                rayPathMinionIn.Init(initWalkNode);
            }
        }


        if (rayPathMinionOut != null)
        {
            rayPathMinionOut.transform.position = endWalkNode.transform.position;
            rayPathMinionOut.Init(endWalkNode);
        }
        else
        {
            var go = GameObject.FindWithTag("ray_path_minion_out");
            if (go == null)
                Debug.LogError("ray path minion out is null!!!");
            else
            {
                rayPathMinionOut = go.GetComponent<MinionInOutElectricity>();
                rayPathMinionOut.transform.position = endWalkNode.transform.position;
                rayPathMinionOut.Init(endWalkNode);
            }
                
        }

    }

    /// <summary>
    /// This will execute when the button in lose or win popups is pressed.
    /// </summary>
    protected void OnFinishLevelCallback(string p)
    {
        Time.timeScale = 1;//TODO:: // SACAR ESTO A LA MIERDA
        _gameManager.SetCurrentLevelInfo(null);
        GameManager.popupManager.DisplayedPopupWasClosed();
        SceneManager.LoadScene("World Selector Screen");
    }
}
