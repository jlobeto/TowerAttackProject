using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerManager : MonoBehaviour
{

    public Level level;
    public Action<TowerType> OnClickTower = delegate { };

    List<TowerBase> _towers;
    TowerBase _previousClickedTower;
    OverchargePilar[] _overchargePilars;

	void Start () 
	{
        _overchargePilars = FindObjectsOfType<OverchargePilar>();
    }
	
	void Update () 
	{
		
	}

    public void StopOrInitTowers(bool enabled = false)
    {
        foreach (var item in _towers)
        {
            item.enabled = enabled;
        }
    }

    public void HideAllTowers()
    {
        foreach (var item in _towers)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void ShowTowerByTypeAndName(TowerType t, string name = "")
    {
        foreach (var item in _towers)
        {
            if(item.towerType == t )
            {
                if(name != "" && item.towerName == name)
                {
                    item.gameObject.SetActive(true);
                }
                else if(name == "")
                {
                    item.gameObject.SetActive(true);
                }
            }
        }
    }

	public TowerBase GetTowerByTypeAndName(TowerType t, string name = "")
    {
		foreach (var item in _towers)
		{
			if(item.towerType == t )
			{
				if(name != "" && item.towerName == name)
				{
					return item;
				}
				else if(name == "")
				{
					return 	item;
				}
			}
		}
		return null;
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
    

    public void DoSwapTowerInitialization(TowerBase toDestroy, TowerBase toAdd)
    {
        OverchargePilar selected = null;
        if(_overchargePilars != null)
        {
            foreach (var pilar in _overchargePilars)
            {
                foreach (var affected in pilar.affected)
                {
                    if (affected.GetInstanceID() == toDestroy.GetInstanceID())
                        selected = pilar;
                }
            }
        }

        _towers.Remove(toDestroy);
        Destroy(toDestroy.gameObject);

        var stat = level.GameManager.TowerLoader.GetStatByLevel(toAdd.towerType, level.levelID);
        toAdd.Initialize(stat);
        _towers.Add(toAdd);

        if (selected != null)
            selected.AddNewAffectedTower(toAdd);
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

        selected.ActivateAttackRangeSprite();

        if (_previousClickedTower != null && _previousClickedTower.GetInstanceID() != selected.GetInstanceID())
            _previousClickedTower.DeactivateAttackRangeSprite();

        OnClickTower(selected.towerType);

        _previousClickedTower = selected;
    }
}
