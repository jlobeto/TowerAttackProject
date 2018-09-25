using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsJsonLoaderManager
{
    Dictionary<MinionType, GenericListJsonLoader<BaseMinionStat>> _allJsons;

    public MinionsJsonLoaderManager()
    {
        _allJsons = new Dictionary<MinionType, GenericListJsonLoader<BaseMinionStat>>();

        var types = Enum.GetValues(typeof(MinionType));
        
        foreach (var item in types)
        {
            MinionType t = (MinionType)item;
            //El tipo T del generic "GenericListJsonLoader<T>" puede ser distinto dependiendo del que minion sea.
            //Posibles 'T' = BaseMinionStat, RunnerMinionStat, TankMinionStat, etc.
            var config = GameUtils.LoadConfig<GenericListJsonLoader<BaseMinionStat>>
                (item.ToString()+".json"
                , GameUtils.MINION_CONFIG_PATH);

            _allJsons.Add(t, config);
        }
        
    }

    public BaseMinionStat GetStatByLevel(MinionType type, int lvlId)
    {
        var stat = _allJsons[type].list.FirstOrDefault(i => i.levelId == lvlId);
        if (stat == null)
            throw new Exception("GetStatByLevel > JSON Object does not exist. There isn't a minion type : "
                + type.ToString() + " with a levelID : "+ lvlId);

        return stat;
    }
}
