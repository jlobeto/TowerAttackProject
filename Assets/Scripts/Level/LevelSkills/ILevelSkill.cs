using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelSkill
{
    List<GameObject> CastSkill(List<GameObject> targets, float t = 0);
    List<GameObject> CastSkill(List<GameObject> targets, float rate, float t = 0);
}
