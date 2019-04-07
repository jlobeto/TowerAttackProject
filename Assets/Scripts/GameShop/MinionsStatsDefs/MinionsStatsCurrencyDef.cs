using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinionsStatsCurrencyDef
{
    public string type;

    //Common variables.
    public int baseHPCurrencyValue;
    public int baseSpeedCurrencyValue;
    public int baseCooldownCurrencyValue;
    public int baseSkillCurrencyValue;

    public StatCurrencyPerLevelDef[] hpCurrencyAddPerLevel;
    public StatCurrencyPerLevelDef[] speedCurrencyAddPerLevel;
    public StatCurrencyPerLevelDef[] cooldownCurrencyAddPerLevel;
    public StatCurrencyPerLevelDef[] skillCurrencyAddPerLevel;

    //
    public int basePassiveCurrencyValue;
    public StatCurrencyPerLevelDef[] passiveCurrencyAddPerLevel;

}
