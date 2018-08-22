using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowLevelSkill : ILevelSkill
{
    public void CastSkill(List<GameObject> targets, float t = 0)
    {
        throw new System.NotImplementedException();
    }

    public void CastSkill(List<GameObject> targets, float rate, float t = 0)
    {
        List<TowerBase> towers = new List<TowerBase>();
        foreach (var item in targets)
        {
            var tower = item.GetComponent<TowerBase>();
            if (tower != null)
                towers.Add(tower);
        }

        foreach (var item in towers)
        {
            item.SlowDebuff(t, rate);
        }
    }
}
