using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public Level level;

    TowerBase[] _towers;

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
        _towers = FindObjectsOfType<TowerBase>();
		for (int i = 0; i < _towers.Length; i++) 
		{
			var t = _towers [i];
			var stat = level.GameManager.TowerLoader.GetStatByLevel (t.towerType, level.levelID);
			t.Initialize (stat);
		}
    }
}
