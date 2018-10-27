using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public Level level;

    List<TowerBase> _towers;

	void Start () 
	{
	}
	
	void Update () 
	{
		
	}

    public void StopTowers()
    {
        foreach (var item in _towers)
        {
            item.enabled = false;
        }
    }

    public void Init()
    {
        _towers = FindObjectsOfType<TowerBase>().ToList();
		for (int i = 0; i < _towers.Count; i++)
		{
			var t = _towers [i];
			var stat = level.GameManager.TowerLoader.GetStatByLevel (t.towerType, level.levelID);
			t.Initialize (stat);
		}
    }

    public void InitSingleTower(TowerBase t)
    {
        var stat = level.GameManager.TowerLoader.GetStatByLevel(t.towerType, level.levelID);
        t.Initialize(stat);
        _towers.Add(t);
    }

    public void DestroySingleTower(TowerBase t)
    {
        _towers.Remove(t);
        Destroy(t.gameObject);
    }

    public List<TowerBase> GetLevelTowersByType(List<TowerType> types)
    {
        var list = new List<TowerBase>();
        foreach (var t in _towers)
        {
            foreach (var item in types)
            {
                if (t.towerType == item && !list.Any(i => i.towerType == t.towerType))
                    list.Add(t);
            }
        }

        return list;
    }

    public void ActivateTowerAttackRange(int id)
    {
        var selected = _towers.FirstOrDefault(i => i.Id == id);
        if (selected == null) return;

        selected.ActivateAttackRangePS();
    }


}
