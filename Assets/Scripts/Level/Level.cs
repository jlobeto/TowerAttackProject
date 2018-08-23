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
    public int wavesAmount;
    [Tooltip("In seconds")]
    public float buildSquadTime = 15;
    [Tooltip("At wave init")]
    public float minionSpawnTime = 1f;
    public List<LevelSkillManager.SkillType> levelSkills = new List<LevelSkillManager.SkillType>();
    public List<Minion> availableMinions = new List<Minion>();

    TowerManager _towerManager;
    MinionManager _minionManager;
    LevelSkillManager _lvlSkillManager;
    LevelGoal _levelGoal;
    LevelCanvasManager _lvlCanvasManager;

    int _currentLevelPoints = 0;

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
        
    }

    void Init()
    {
        var gameplayManagersGO = new GameObject("GameplayManagers");
        _minionManager = gameplayManagersGO.AddComponent<MinionManager>();
        _towerManager = gameplayManagersGO.AddComponent<TowerManager>();
        _lvlSkillManager = gameplayManagersGO.AddComponent<LevelSkillManager>();

        _towerManager.level = _lvlSkillManager.level = _minionManager.level = this;
        
        _currentLevelPoints = initialLevelPoints;

        InitLevelGoal();
        InitLevelCanvas();
    }

    public void BuildMinion(MinionType t)
    {
        if (!CheckMinionSale(t)) return;

        var cost = _minionManager.GetMinionPrice(t);
        _lvlCanvasManager.UpdateLevelPointBar(_currentLevelPoints - cost, initialLevelPoints);
        _currentLevelPoints -= cost;
        _minionManager.SpawnMinion(t);
        _minionManager.SetNextMinionFree();
    }
        
    bool CheckMinionSale(MinionType t)
    {
        var cost = _minionManager.GetMinionPrice(t);
        return _currentLevelPoints - cost >= 0;
    }

    public void UpdateLevelGoal()
    {
        _levelGoal.UpdateGoal(-1);
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
        _lvlCanvasManager.BuildAvailablesMinions(availableMinions.Select(i => i.minionType).ToList());
        _lvlCanvasManager.level = this;
    }

    void GoalCompletedHandler()
    {
        Debug.Log("----- Level Completed -----");
    }
}
