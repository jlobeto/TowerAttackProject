﻿using System;
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
    public Action<MinionType> OnMinionDeath = delegate { };
    public Action<MinionType> OnMinionSkillSelected = delegate { };


    List<Minion> _minions = new List<Minion>();
    int _deathCount;
    int _successCount;
    GameObject _allMinions;
    List<EnvironmentBridge> _levelBridges;

    public void SpawnMinion(MinionType type, Vector3 spawnPos, Minion available)
    {
        Minion minion = null;

        minion = Instantiate(available, spawnPos, Quaternion.identity);

        SetMinionStats(ref minion);

        minion.minionManager = this;
        minion.transform.SetParent(_allMinions.transform);
        minion.OnWalkFinished += MinionWalkFinishedHandler;
        minion.OnDeath += MinionDeathHandler;
        minion.OnMinionSkill += MinionSkillActivatedHandler;

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

    public void OnBridgeEnable(List<EnvironmentBridge> bridges)
    {
        _levelBridges = bridges;
    }

    /// <summary>
    /// For bridge event. 
    /// </summary>
    public bool MinionHasToFall(GroundMinion m, WalkNode nextWalkNode)
    {
        if (nextWalkNode.levelEventBridgeNodeName == "" || nextWalkNode.levelEventBridgeNodeName.Contains("pivot"))
            return false;

        foreach (var bridge in _levelBridges)
        {
            if(nextWalkNode.levelEventBridgeNodeName == bridge.destinationA.levelEventBridgeNodeName)
            {
                var isInsideBridge = bridge.bridge_B_GameObject.IsMinionInsideBridge(m);
                return isInsideBridge && !bridge.isPointingA;
            }
            else if (nextWalkNode.levelEventBridgeNodeName == bridge.destinationB.levelEventBridgeNodeName)
            {
                var isInsideBridge = bridge.bridge_A_GameObject.IsMinionInsideBridge(m);
                return isInsideBridge && bridge.isPointingA;
            }
        }

        return false;
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

	public Minion GetMinion(MinionType t)
	{
		return _minions.FirstOrDefault (i => i.minionType == t);
	}

    public void AffectMinions(Action<Minion> action)
    {
        foreach (var m in _minions)
        {
            action(m);
        }
    }

    public void DeleteAllMinionsInPath()
    {
        _minions = new List<Minion>();
        Destroy(_allMinions);
        Init();
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
        OnMinionDeath(m.minionType);
        int toAdd = Mathf.RoundToInt(m.pointsValue * m.levelPointsToRecover * .75f);
        level.UpdatePoints(toAdd);
        _deathCount++;
        DestroyMinion(m);
        
    }

    void MinionSkillActivatedHandler(MinionType t)
    {
        SoundFxNames sound = SoundFxNames.none;
        switch (t)
        {
            case MinionType.Runner:
                sound = SoundFxNames.skill_runner;
                break;
            case MinionType.Tank:
                sound = SoundFxNames.skill_tank;
                break;
            case MinionType.Dove:
                sound = SoundFxNames.skill_dove;
                break;
            case MinionType.Healer:
                sound = SoundFxNames.skill_healer;
                break;
            case MinionType.Zeppelin:
                sound = SoundFxNames.skill_zeppelin;
                break;
            case MinionType.WarScreamer:
                sound = SoundFxNames.skill_warscreamer;
                break;
            default:
                break;
        }
        SoundManager.instance.PlaySound(sound);

        if (level.levelMode != LevelMode.Tutorial) return;

        OnMinionSkillSelected(t);
    }
    #endregion

    void DestroyMinion(Minion m)
    {
        m.OnWalkFinished -= MinionWalkFinishedHandler;
        m.OnDeath -= MinionDeathHandler;
        m.OnMinionSkill -= MinionSkillActivatedHandler;

        _minions.Remove(m);
        Destroy(m.gameObject);
    }

    void SetMinionStats(ref Minion minion)
    {
        BaseMinionStat stats;
        var type = minion.minionType == MinionType.MiniZeppelin ? MinionType.Zeppelin : minion.minionType;//only yo get stats of zepp because minizep stats are inside zep
        var bought = level.GameManager.User.GetMinionBought(type);

        //HP STATS
        stats = level.GameManager.MinionsJsonLoader.GetStatByLevel(type, bought.hp);
        minion.hp = stats.hp;

        //These 5 are always the same
        minion.pointsValue = stats.pointsValue;
        minion.levelPointsToRecover = stats.levelPointsToRecover;
        minion.spawnCooldown = stats.spawnCooldown;
        minion.skillTime = stats.skillTime;
        minion.skillCooldown = stats.skillCooldown;

        //SPEED STATS
        stats = level.GameManager.MinionsJsonLoader.GetStatByLevel(type, bought.speed);
        minion.speed = stats.speed;

        //SKILL STATS
        stats = level.GameManager.MinionsJsonLoader.GetStatByLevel(type, bought.skill);

        switch (minion.minionType)
        {
            case MinionType.Runner:
                (minion as Runner).skillDeltaSpeed = stats.skillDeltaSpeed;
                break;
            case MinionType.Tank:
                (minion as Tank).shieldHits = stats.shieldHits;
                (minion as Tank).skillArea = stats.skillArea;
                break;
            case MinionType.Dove:
                minion.skillCooldown = stats.skillCooldown;
                break;
            case MinionType.Healer:
                (minion as Healer).areaOfEffect = stats.areaOfEffect;
                (minion as Healer).healPerSecond = stats.healPerSecond;
                (minion as Healer).skillHealPercent = stats.skillHealAmount;
                break;
            case MinionType.MiniZeppelin:
                minion.hp = stats.miniZeppelinStat.hitsToDie;
                minion.speed = stats.miniZeppelinStat.speed;
                break;
            case MinionType.Zeppelin:
                (minion as Zeppelin).miniZeppelinCount = stats.miniZeppelinCount;
                (minion as Zeppelin).skillMiniZepCount = stats.skillMiniZepCount;
                break;
			case MinionType.WarScreamer:
				(minion as WarScreamer).areaOfEffect = stats.areaOfEffect;
				(minion as WarScreamer).activeSpeedDelta = stats.activeSpeedDelta;
                break;
            default:
                break;
        }
    }
}
