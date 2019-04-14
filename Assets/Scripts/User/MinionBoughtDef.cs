using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This item will be persisted on the device.
/// Each time user buys a new minion, this class will be created and saved.
/// Each time user upgrades an stat, the upgraded one will save the level of its.
/// </summary>
[Serializable]
public class MinionBoughtDef
{
    public string type;
    public int hp;
    public int speed;
    public int skill;
    public int passiveSkill;
}
