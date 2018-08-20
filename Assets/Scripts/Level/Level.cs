using System.Collections;
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

    int _currentLevelPoints = 0;

    /// <summary>
    /// Used to inform CurrentLevelPoints to user on the GUI.
    /// </summary>
    public int CurrentLevelPoints { get { return _currentLevelPoints; } }


    void Start () {
        Init();
    }

	void Update ()
    {
        MinionType t = MinionType.Runner;
        bool pressed = false;

        if (Input.GetMouseButtonDown(0))
        {
            pressed = true;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            pressed = true;
            t = MinionType.Tank;
        }
        else if (Input.GetMouseButtonDown(2))
        {
            pressed = true;
            t = MinionType.Dove;
        }

        if (pressed)
        {
            _minionManager.SpawnMinion(t);
            _minionManager.SetNextMinionFree();
        }
        
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
       

    void GoalCompletedHandler()
    {
        Debug.Log("----- Level Completed -----");
    }
}
