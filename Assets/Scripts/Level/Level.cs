using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelID = "test01";
    public List<WalkNode> initialWalkNodes = new List<WalkNode>();
    [Tooltip("Ones camera is set, asign to this")]
    public Transform cameraTransform;

    public int initialLevelPoints;
    [Tooltip("In seconds")]
    public float levelTime = 60;
    public List<LevelSkillManager.SkillType> levelSkills = new List<LevelSkillManager.SkillType>();
    public List<Minion> availableMinions = new List<Minion>();

    //[HideInInspector]
    //public bool startMinionSpawning;

    TowerManager _towerManager;
    MinionManager _minionManager;
    LevelSkillManager _lvlSkillManager;
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
    

    void Start () {
        Init();
    }

	void Update ()
    {
        if(_levelEnded) return;

        GameObjectSelection();
        OnRunLevelTimer();
    }

    void OnRunLevelTimer()
    {
        _levelTimeAux -= Time.deltaTime;
        _lvlCanvasManager.UpdateLevelTimer(_levelTimeAux < 0 ? 0 : _levelTimeAux);
        if (_levelTimeAux < 0)
        {
            _levelEnded = true;
            var popupMan = FindObjectOfType<PopupManager>();//esto no vá. mas adelante voy a crear un gamemanager que tenga este coso
            popupMan.BuildEndLevelPopup(_lvlCanvasManager.transform);
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
    public bool BuildMinion(MinionType t, bool builtTime = false)
    {
        if (!CheckMinionSale(t)) return false;
        var cost = _minionManager.GetMinionPrice(t);
        UpdatePoints(-cost);
        /*_lvlCanvasManager.UpdateLevelPointBar(_currentLevelPoints - cost, initialLevelPoints);
        _currentLevelPoints -= cost;*/
        _minionManager.SpawnMinion(t, builtTime);
        if(!builtTime)
            _minionManager.SetNextMinionFree();

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
        var gameplayManagersGO = new GameObject("GameplayManagers");
        _minionManager = gameplayManagersGO.AddComponent<MinionManager>();
        _towerManager = gameplayManagersGO.AddComponent<TowerManager>();
        _lvlSkillManager = gameplayManagersGO.AddComponent<LevelSkillManager>();
        _goSelector = FindObjectOfType<GameObjectSelector>();
        _floorEffect = FindObjectOfType<FloorEffect>();

        _towerManager.level = _lvlSkillManager.level = _minionManager.level = this;

        _currentLevelPoints = initialLevelPoints;
        _levelTimeAux = levelTime;

        InitLevelGoal();
        InitLevelCanvas();

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
        _lvlCanvasManager.BuildAvailableMinionsButtons(availableMinions);
        _lvlCanvasManager.level = this;
        _lvlCanvasManager.UpdateLevelTimer(levelTime);
        _lvlCanvasManager.UpdateLevelLives(_levelGoal.CurrentLives, _levelGoal.lives);
        UpdatePoints(0);
    }

    #endregion


    public void UpdateLevelGoal()
    {
        _levelGoal.UpdateGoal(-1);
        _lvlCanvasManager.UpdateLevelLives(_levelGoal.CurrentLives, _levelGoal.lives);
        _floorEffect.InitAnimation();
    }

    void GoalCompletedHandler()
    {
        Debug.Log("----- Level Completed -----");
    }
    
    public void UpdatePoints(int points)
    {
        var prevPoints = _currentLevelPoints;
        _currentLevelPoints += points;
        if (_currentLevelPoints > initialLevelPoints)
            _currentLevelPoints = initialLevelPoints;

        _lvlCanvasManager.UpdateLevelPointBar(_currentLevelPoints, prevPoints, initialLevelPoints);
    }

    #region Commented Functions
    /// <summary>
    /// This is to spawn minions within a squadTimer.
    /// So it will spawn minions selected one per one. Not necesary right now.
    /// </summary>
    /*void OnSpawnMinions()
    {
        if (!startMinionSpawning) return;
        _minionSpawnTimeAux -= Time.deltaTime;
        if (_minionSpawnTimeAux < 0)
        {
            var continueSpawning = _minionManager.SetNextMinionFree();
            if (!continueSpawning)
                startMinionSpawning = false;

            _minionSpawnTimeAux = minionSpawnTime;
        }
    }*/

    #endregion

}
