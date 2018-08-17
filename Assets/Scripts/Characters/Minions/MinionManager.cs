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
        switch (type)
        {
            case MinionType.Runner:
                var available = level.availableMinions.FirstOrDefault(m => m.GetType() == typeof(Runner));
                minion = Instantiate<Runner>((Runner)available, level.initialWalkNodes[0].transform.position, Quaternion.identity);
                break;
            case MinionType.Tank:
                break;
            case MinionType.Healer:
                break;
            case MinionType.Hero:
                break;
            case MinionType.Ghost:
                break;
            case MinionType.WarScreammer:
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
        _minions.Add(minion);
    }

    /// <summary>
    /// Will release and set minion walk to true. One minion at a time;
    /// </summary>
    public void SetNextMinionFree()
    {
        var minion = _minions.FirstOrDefault(m => !m.CanWalk);
        if(minion != null)
            minion.SetWalk(true);
    }

    void Start () {
        Init();
	}
	
	void Update () {
		
	}

    void Init()
    {
        _allMinions = new GameObject("All Minions");
    }

    void MinionWalkFinishedHandler()
    {
        Debug.Log("MinionWalkFinishedHandler");
        level.UpdateLevelGoal();
    }
}
