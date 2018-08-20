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

    void Init()
    {
        _towers = FindObjectsOfType<TowerBase>();
    }
}
