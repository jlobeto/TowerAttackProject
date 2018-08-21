using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSkill
{
    void CastSkill(List<TowerBase> towers);
    void CastSkill(List<Minion> minions);
}
