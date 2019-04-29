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

    public enum StatNames
    {
        HP,
        SPD,
        PASSIVE,
        SKILL
    }

    public int GetStatLevel(StatNames id)
    {
        switch (id)
        {
            case StatNames.HP:
                return hp;
                break;
            case StatNames.SPD:
                return speed;
                break;
            case StatNames.PASSIVE:
                return passiveSkill;
                break;
            case StatNames.SKILL:
                return skill;
                break;
            default:
                break;
        }

        return 1;
    }

    public void IncrementLevelToStat(StatNames id)
    {
        switch (id)
        {
            case StatNames.HP:
                hp++;
                break;
            case StatNames.SPD:
                speed++;
                break;
            case StatNames.PASSIVE:
                passiveSkill++;
                break;
            case StatNames.SKILL:
                skill++;
                break;
        }


    }
}
