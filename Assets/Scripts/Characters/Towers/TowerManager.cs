using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public Level level;

    TowerBase[] _towers;

	void Start () {
        Init();
	}
	
	void Update () {
		
	}

    public void StopTowers()
    {
        foreach (var item in _towers)
        {
            item.enabled = false;
        }
    }

    void Init()
    {
        _towers = FindObjectsOfType<TowerBase>();
    }
}
