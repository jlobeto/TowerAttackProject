using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    [HideInInspector]
    public Level level;
    public Action<MinionType> OnNewMinionSpawned = delegate { };
    public Action<MinionType> OnMinionWalkFinished = delegate { };


    List<Minion> _minions = new List<Minion>();
    int _deathCount;
    int _successCount;
    GameObject _allMinions;


    public void SpawnMinion(MinionType type, Vector3 spawnPos, Minion available)
    {
        Minion minion = null;

        minion = Instantiate(available, spawnPos, Quaternion.identity);

        if(type == MinionType.Healer )
            (minion as Healer).manager = this;
        else if(type == MinionType.Zeppelin)
            (minion as Zeppelin).manager = this;
		else if(type == MinionType.WarScreamer)
			(minion as WarScreamer).manager = this;

        if (minion == null)
        {
            Debug.LogError("Error creating a Minion");
            return;
        }

        ModifyMinionStatByLevelID(ref minion, level.levelID);
        minion.transform.SetParent(_allMinions.transform);
        minion.OnWalkFinished += MinionWalkFinishedHandler;
        minion.OnDeath += MinionDeathHandler;
        _minions.Add(minion);
        OnNewMinionSpawned(type);
    }
    
    /// <summary>
    /// Will release and set minion walk to true. One minion at a time;
    /// </summary>
    public bool SetNextMinionFree(WalkNode node, Vector3 position = default(Vector3))
    {
        foreach (var minion in _minions)
        {
            if (minion != null && !minion.hasBeenFreed && !minion.IsDead)
            {
                minion.InitMinion(node, position);
                minion.SetWalk(true);
                return true;
            }
        }
        return false; //there is not a minion deactivated   
    }

    public int GetMinionPrice(MinionType t)
    {
        foreach (var item in level.availableMinions)
        {
            if (item.minionType == t)
                return item.pointsValue;
        }

        throw new System.Exception("There is not an available minion of type " + t.ToString());
    }

    public void ChangeMinionOrder(int from, int to)
    {
        var minionFrom =_minions[from];
        var minionTo = _minions[to];

        _minions[from] = minionTo;
        _minions[to] = minionFrom;
    }

    public int DeleteMinionByIndex(int index)
    {
        if (index >= _minions.Count) return 0;

        var minion = _minions[index];
        var pointsValue = minion.pointsValue;
        _minions.RemoveAt(index);
        Destroy(minion.gameObject);
        return pointsValue;
    }

    public void StopMinions()
    {
        foreach (var item in _minions)
        {
            item.enabled = false;
        }
    }


    void Start() {
        Init();
    }

    void Update() {
        
    }

    void Init()
    {
        _allMinions = new GameObject("All Minions");
    }

    public List<Minion> GetMinions(Func<Minion,bool> func)
    {
        var list = new List<Minion>();

        foreach (var item in _minions)
        {
            if (func(item))
                list.Add(item);
        }

        return list;
    }

    public void AffectMinions(Action<Minion> action)
    {
        foreach (var m in _minions)
        {
            action(m);
        }
    }

    /// <summary>
    /// When a minion is already released and is walking throught the path it can be selected to pop a number of
    /// differents actions.
    /// </summary>
    public void OnReleasedMinionSelected(int instanceID)
    {
        var selected = _minions.FirstOrDefault(i => i.gameObject.GetInstanceID() == instanceID && !i.IsDead);
        if (selected == null) return;

        selected.ActivateSelfSkill();

    }

    #region Handlers
    void MinionWalkFinishedHandler(Minion m)
    {
        int toAdd = Mathf.RoundToInt( m.pointsValue * m.levelPointsToRecover);
        level.UpdatePoints(toAdd);
        DestroyMinion(m);
        _successCount++;
        level.UpdateLevelGoal();
        OnMinionWalkFinished(m.minionType);
    }

    void MinionDeathHandler(Minion m)
    {
        int toAdd = Mathf.RoundToInt(m.pointsValue * m.levelPointsToRecover * .75f);
        level.UpdatePoints(toAdd);
        _deathCount++;
        DestroyMinion(m);
    }
    #endregion

    void DestroyMinion(Minion m)
    {
        _minions.Remove(m);
        Destroy(m.gameObject);
    }

    void ModifyMinionStatByLevelID(ref Minion minion, int levelId)
    {
        BaseMinionStat stats;
        if(minion.minionType != MinionType.MiniZeppelin)
            stats = level.GameManager.MinionsLoader.GetStatByLevel(minion.minionType, levelId);
        else
            stats = level.GameManager.MinionsLoader.GetStatByLevel(MinionType.Zeppelin, levelId);
        
        minion.hp = stats.hp;
        minion.pointsValue = stats.pointsValue;
        minion.levelPointsToRecover = stats.levelPointsToRecover;
        minion.speed = stats.speed;
        minion.strength = stats.strength;
        minion.spawnCooldown = stats.spawnCooldown;
        minion.skillTime = stats.skillTime;
        minion.skillCooldown = stats.skillCooldown;

        switch (minion.minionType)
        {
            case MinionType.Runner:
                (minion as Runner).skillDeltaSpeed = stats.skillDeltaSpeed;
                break;
            case MinionType.Tank:
                (minion as Tank).shieldHits = stats.shieldHits;
                (minion as Tank).skillArea = stats.skillArea;
                break;
            case MinionType.Dove://does not have any special stats
                break;
            case MinionType.Healer:
                (minion as Healer).areaOfEffect = stats.areaOfEffect;
                (minion as Healer).healPerSecond = stats.healPerSecond;
                (minion as Healer).skillHealPercent = stats.skillHealAmount;
                break;
            case MinionType.MiniZeppelin:
                minion.hp = stats.miniZeppelinStat.hitsToDie;
                minion.pointsValue = stats.miniZeppelinStat.pointsValue;
                minion.levelPointsToRecover = stats.miniZeppelinStat.levelPointsToRecover;
                minion.speed = stats.miniZeppelinStat.speed;
                break;
            case MinionType.Zeppelin:
                (minion as Zeppelin).miniZeppelinCount = stats.miniZeppelinCount;
                (minion as Zeppelin).skillMiniZepCount = stats.skillMiniZepCount;
                break;
			case MinionType.WarScreamer:
				(minion as WarScreamer).areaOfEffect = stats.areaOfEffect;
				(minion as WarScreamer).activeSpeedDelta = stats.activeSpeedDelta;
				(minion as WarScreamer).passiveSpeedDelta = stats.passiveSpeedDelta;
				(minion as WarScreamer).lifePercentThresholdToActivatePassive = stats.lifePercentThresholdToActivatePassive;
				(minion as WarScreamer).passiveSkillDurationOnAffectedMinion = stats.passiveSkillDurationOnAffectedMinion;
				(minion as WarScreamer).timeToPassive = stats.timeToPassive;
                break;
            default:
                break;
        }
    }
}
