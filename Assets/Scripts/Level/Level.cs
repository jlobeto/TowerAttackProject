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
    public float buildSquadTime = 15;
    [Tooltip("In seconds")]
    public float levelTime = 60;
    [Tooltip("At wave init")]
    public float minionSpawnTime = 1f;
    public List<LevelSkillManager.SkillType> levelSkills = new List<LevelSkillManager.SkillType>();
    public List<Minion> availableMinions = new List<Minion>();

    [HideInInspector]
    public bool startMinionSpawning;

    TowerManager _towerManager;
    MinionManager _minionManager;
    LevelSkillManager _lvlSkillManager;
    LevelGoal _levelGoal;
    LevelCanvasManager _lvlCanvasManager;

    int _currentLevelPoints = 0;
    float _minionSpawnTimeAux;

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
        OnSpawnMinions();
    }



    #region Minion Spawning Stuff
    
    /// <returns>True if minion has been created</returns>
    public bool BuildMinion(MinionType t, bool builtTime)
    {
        if (!CheckMinionSale(t)) return false;
        var cost = _minionManager.GetMinionPrice(t);
        _lvlCanvasManager.UpdateLevelPointBar(_currentLevelPoints - cost, initialLevelPoints);
        _currentLevelPoints -= cost;
        _minionManager.SpawnMinion(t, builtTime);
        if(!builtTime)
            _minionManager.SetNextMinionFree();

        return true;
    }

    void OnSpawnMinions()
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

    #endregion
    
    #region Inits()
    void Init()
    {
        var gameplayManagersGO = new GameObject("GameplayManagers");
        _minionManager = gameplayManagersGO.AddComponent<MinionManager>();
        _towerManager = gameplayManagersGO.AddComponent<TowerManager>();
        _lvlSkillManager = gameplayManagersGO.AddComponent<LevelSkillManager>();

        _towerManager.level = _lvlSkillManager.level = _minionManager.level = this;

        _currentLevelPoints = initialLevelPoints;
        _minionSpawnTimeAux = minionSpawnTime;

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
        _lvlCanvasManager.BuildAvailableMinionsButtons(availableMinions.Select(i => i.minionType).ToList());
        _lvlCanvasManager.level = this;
        _lvlCanvasManager.SetBuildSquadTimer(buildSquadTime,levelTime);
    }

    #endregion


    public void UpdateLevelGoal()
    {
        _levelGoal.UpdateGoal(-1);
    }

    void GoalCompletedHandler()
    {
        Debug.Log("----- Level Completed -----");
    }
}
