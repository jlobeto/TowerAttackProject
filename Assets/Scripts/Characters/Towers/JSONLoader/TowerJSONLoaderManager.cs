using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TowerJSONLoaderManager 
{

	Dictionary<TowerType, GenericListJsonLoader<TowerStat>> _allJsons;

	public TowerJSONLoaderManager()
	{
		_allJsons = new Dictionary<TowerType, GenericListJsonLoader<TowerStat>>();

		var types = Enum.GetValues(typeof(TowerType)).Cast<TowerType>().ToList();

		foreach (var item in types)
		{
			//Debug.Log(item);
			var config = GameUtils.LoadConfig<GenericListJsonLoader<TowerStat>>
				(item.ToString()+".json"
					, GameUtils.TOWER_CONFIG_PATH);

			_allJsons.Add(item, config);
		}
	}

	public TowerStat GetStatByLevel(TowerType type, int lvlId)
	{
		var stat = _allJsons[type].list.FirstOrDefault(i => i.levelId == lvlId);
		if (stat == null)
			throw new Exception("GetStatByLevel > JSON Object does not exist. There isn't a tower type : "
				+ type.ToString() + " with a levelID : "+ lvlId);

		return stat;
	}
}
