using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
    public Level level;

    List<Minion> _minions;
    int _deathCount;
    int _successCount;

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
        _minions.Add(minion);
    }

    void Start () {
        
	}
	
	void Update () {
		
	}
    
}
