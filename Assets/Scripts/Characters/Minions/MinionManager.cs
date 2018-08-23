using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public Level level;

    List<Minion> _minions = new List<Minion>();
    int _deathCount;
    int _successCount;
    GameObject _allMinions;

    public void SpawnMinion(MinionType type)
    {
        Minion minion = null;
        Minion available = null;
        switch (type)
        {
            case MinionType.Runner:
            case MinionType.Tank:
                available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Minion) && m.minionType == type);
                minion = Instantiate<Minion>(available, level.initialWalkNodes[0].transform.position, Quaternion.identity);
                break;
            case MinionType.Dove:
                available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Dove));
                minion = Instantiate<Dove>((Dove)available, level.initialWalkNodes[0].transform.position, Quaternion.identity);
                break;
            case MinionType.Zeppelin:
                break;
            case MinionType.FatTank:
                break;
            case MinionType.GoldDigger:
                break;
            case MinionType.Healer:
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

        minion.InitMinion(level.initialWalkNodes[0]);
        minion.transform.SetParent(_allMinions.transform);
        minion.OnWalkFinished += MinionWalkFinishedHandler;
        minion.OnDeath += MinionDeathHandler;
        _minions.Add(minion);
    }

    /// <summary>
    /// Will release and set minion walk to true. One minion at a time;
    /// </summary>
    public void SetNextMinionFree()
    {
        var minion = _minions.FirstOrDefault(m => !m.CanWalk);
        if (minion != null)
            minion.SetWalk(true);
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

    void Start() {
        Init();
    }

    void Update() {

    }

    void Init()
    {
        _allMinions = new GameObject("All Minions");
    }

    void MinionWalkFinishedHandler(Minion m)
    {
        DestroyMinion(m);
        _successCount++;
        level.UpdateLevelGoal();
    }

    void MinionDeathHandler(Minion m)
    {
        _deathCount++;
        DestroyMinion(m);
    }

    void DestroyMinion(Minion m)
    {
        _minions.Remove(m);
        Destroy(m.gameObject);
    }
}
