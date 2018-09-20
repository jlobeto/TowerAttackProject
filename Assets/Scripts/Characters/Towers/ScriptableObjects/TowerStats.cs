using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerStat", menuName = "ScriptableObjects/Tower Stats")]
public class TowerStats : ScriptableObject
{
    //this values MUST NOT be modified, because it gonna affect the scriptable object for ever.
    public float fireRate = 1f;
    public float fireRange = 8f;
    public float bulletDamage = 5;
    public float bulletRange = 0;
    public int level = 1;
    public float rotationSpeed = 5f;
}
