using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public Level level;
    public Action OnNewMinionSpawned = delegate { };


    List<Minion> _minions = new List<Minion>();
    int _deathCount;
    int _successCount;
    GameObject _allMinions;
    

    public void SpawnMinion(MinionType type, bool isBuiltTime)
    {
        
        Minion minion = null;
        Minion available = null;

        Vector3 spawnPos = isBuiltTime ? new Vector3(1000, 1000, 1000) : level.initialWalkNodes[0].transform.position;
        switch (type)
        {
            case MinionType.Runner:
                available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Runner));
                minion = Instantiate<Runner>((Runner)available, spawnPos, Quaternion.identity);
                break;
            case MinionType.Tank:
                available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Tank));
                minion = Instantiate<Tank>((Tank)available, spawnPos, Quaternion.identity);
                break;
            case MinionType.Dove:
                available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Dove));
                minion = Instantiate<Dove>((Dove)available, spawnPos, Quaternion.identity);
                break;
            case MinionType.Zeppelin:
                break;
            case MinionType.FatTank:
                break;
            case MinionType.GoldDigger:
                break;
            case MinionType.Healer:
                available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Healer));
                minion = Instantiate<Healer>((Healer)available, spawnPos, Quaternion.identity);
                (minion as Healer).manager = this;
                break;
            case MinionType.Ghost:
                break;
            case MinionType.WarScreammer:
                break;
            case MinionType.Eagle:
                break;
            case MinionType.Clown:
                break;
            default:
                break;
        }

        if (minion == null)
        {
            Debug.LogError("Error creating a Minion");
            return;
        }
        
        minion.transform.SetParent(_allMinions.transform);
        minion.OnWalkFinished += MinionWalkFinishedHandler;
        minion.OnDeath += MinionDeathHandler;
        _minions.Add(minion);
        OnNewMinionSpawned();
    }

    /// <summary>
    /// Will release and set minion walk to true. One minion at a time;
    /// </summary>
    public bool SetNextMinionFree()
    {
        var minion = _minions.FirstOrDefault(m => !m.CanWalk);
        if (minion != null)
        {
            minion.InitMinion(level.initialWalkNodes[0]);
            minion.SetWalk(true);
            return true;

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
        var selected = _minions.FirstOrDefault(i => i.gameObject.GetInstanceID() == instanceID);
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
}
