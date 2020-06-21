using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinionsStatsCurrencyDef
{
    public string type;
    
    public StatCurrencyPerLevelDef[] hpPerLevel;
    public StatCurrencyPerLevelDef[] speedPerLevel;
    public StatCurrencyPerLevelDef[] skillPerLevel;
    public StatCurrencyPerLevelDef[] passivePerLevel;


    public int GetPrice(MinionBoughtDef.StatNames statName, int lvl)
    {
        int price = -1;
        var levelToSearch = lvl - 1;
        switch (statName)
        {
            case MinionBoughtDef.StatNames.HP:
                price = hpPerLevel.Length >= levelToSearch ? hpPerLevel[levelToSearch].newPrice : -99;
                break;
            case MinionBoughtDef.StatNames.SPD:
                price = speedPerLevel.Length >= levelToSearch ? speedPerLevel[levelToSearch].newPrice : -99;
                break;
            case MinionBoughtDef.StatNames.PASSIVE:
                price = passivePerLevel.Length >= levelToSearch ? passivePerLevel[levelToSearch].newPrice : -99;
                break;
            case MinionBoughtDef.StatNames.SKILL:
                price = skillPerLevel.Length >= levelToSearch ? skillPerLevel[levelToSearch].newPrice : -99;
                break;
        }

        return price;
    }
}
