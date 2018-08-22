using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSkill
{
    void CastSkill(List<GameObject> targets, float t = 0);
    void CastSkill(List<GameObject> targets, float rate ,float t = 0);
}
