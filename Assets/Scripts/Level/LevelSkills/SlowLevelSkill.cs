using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlowLevelSkill : ILevelSkill
{
    public List<GameObject> CastSkill(List<GameObject> targets, float t = 0)
    {
        throw new System.NotImplementedException();
    }

    public List<GameObject> CastSkill(List<GameObject> targets, float rate, float t = 0)
    {
        List<TowerBase> towers = new List<TowerBase>();
        TowerBase tower;
        foreach (var item in targets)
        {
            tower = item.GetComponent<TowerBase>();
            if (tower != null && !(tower is IceTower))
                towers.Add(tower);
        }

        foreach (var item in towers)
        {
            item.SlowDebuff(t, rate);
        }

        return towers.Select(i => i.gameObject).ToList();
    }
}
