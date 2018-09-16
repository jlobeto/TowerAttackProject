using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class StunLevelSkill : ILevelSkill
{
    public List<GameObject> CastSkill(List<GameObject> targets, float time = 0)
    {
        List<TowerBase> towers = new List<TowerBase>();
        foreach (var item in targets)
        {
            var t = item.GetComponent<TowerBase>();
            if (t != null)
                towers.Add(t);
        }

        foreach (var item in towers)
        {
            item.ReceiveStun(time);
        }

        return towers.Select(i => i.gameObject).ToList();
    }
    public List<GameObject> CastSkill(List<GameObject> targets, float rate, float t = 0)
    {
        throw new System.NotImplementedException();
    }

}
