using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceTowerStat", menuName = "ScriptableObjects/Ice Tower Stats")]
public class IceTowerStats : TowerStats
{
    [Range(0f, 1f)]
    public float deltaSpeed = 0.4f;
}
