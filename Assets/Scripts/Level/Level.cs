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
    public LevelGoal levelGoal;
    public List<LevelSkillManager.SkillType> levelSkills = new List<LevelSkillManager.SkillType>();
    public List<Minion> availableMinions = new List<Minion>();

    TowerManager _towerManager;
    MinionManager _minionManager;
    LevelSkillManager _lvlSkillManager;

    int _currentLevelPoints = 0;
    float time = 2f;

    /// <summary>
    /// Used to inform CurrentLevelPoints to user on the GUI.
    /// </summary>
    public int CurrentLevelPoints { get { return _currentLevelPoints; } }


    void Start () {
        Init();
    }
	
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            _minionManager.SpawnMinion(MinionType.Runner);
        }

        time -= Time.deltaTime;
        if (time < 0)
        {
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
        
        levelGoal.OnGoalComplete += GoalCompletedHandler;
    }
    
    void GoalCompletedHandler()
    {
        Debug.Log("----- Level Completed -----");
    }
}
