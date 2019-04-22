using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinionsStatsCurrencyDef
{
    public string type;

    //Common variables.
    public float baseHPCurrencyValue;
    public float baseSpeedCurrencyValue;
    public float baseSkillCurrencyValue;

    public StatCurrencyPerLevelDef[] hpCurrencyAddPerLevel;
    public StatCurrencyPerLevelDef[] speedCurrencyAddPerLevel;
    public StatCurrencyPerLevelDef[] skillCurrencyAddPerLevel;

    //
    public float basePassiveCurrencyValue;
    public StatCurrencyPerLevelDef[] passiveCurrencyAddPerLevel;


    public float GetPrice(MinionBoughtDef.StatNames statName, int lvl)
    {
        float price = 5;
        StatCurrencyPerLevelDef[] currencyPerLvl = new StatCurrencyPerLevelDef[0];

        switch (statName)
        {
            case MinionBoughtDef.StatNames.HP:
                price = baseHPCurrencyValue;
                currencyPerLvl = hpCurrencyAddPerLevel;
                break;
            case MinionBoughtDef.StatNames.SPD:
                price = baseSpeedCurrencyValue;
                currencyPerLvl = speedCurrencyAddPerLevel;
                break;
            case MinionBoughtDef.StatNames.PASSIVE:
                price = basePassiveCurrencyValue;
                currencyPerLvl = passiveCurrencyAddPerLevel;
                break;
            case MinionBoughtDef.StatNames.SKILL:
                price = baseSkillCurrencyValue;
                currencyPerLvl = skillCurrencyAddPerLevel;
                break;
        }

        if (lvl == 2)
            return price;



        return PriceAccumulated(currencyPerLvl, lvl, price);
    }

    float PriceAccumulated(StatCurrencyPerLevelDef[] currencyPerLvl, int lvl , float price)
    {
        float result = price;
        
        for (int i = 0; i < currencyPerLvl.Length; i++)
        {
            var stat = currencyPerLvl[i];
            for (int j = 0; j < stat.levels.Length; j++)
            {
                if (stat.levels[j] > lvl)
                    break;

                result = result + (result * stat.toAdd);
            }
        }

        Debug.Log(result);

        return result;
    }
}
