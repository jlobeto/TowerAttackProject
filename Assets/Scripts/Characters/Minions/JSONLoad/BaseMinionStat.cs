using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseMinionStat
{
    public int levelId;

    //Base Minion
    public int hp;
    public float speed;
    public float strength;
    public int pointsValue;
    public float spawnCooldown;
    public float levelPointsToRecover;
    public float skillTime;
    public float skillCooldown;

    //Runner
    public float skillDeltaSpeed;

    //Tank
    public int shieldHits;
    public float skillArea;

    //Healer && warscreamer
    public float areaOfEffect;

    //Healer
    public float healPerSecond;
    public int skillHealAmount;

    //Zeppelin
    public MiniZeppelinStat miniZeppelinStat;
    public int miniZeppelinCount;
    public int skillMiniZepCount;

    //warscreamer
    public float activeSpeedDelta;
    public float passiveSpeedDelta;
    public float lifePercentThresholdToActivatePassive;
    public float passiveSkillDurationOnAffectedMinion;
    public float timeToPassive;

    public float GetStatByStatId(MinionBoughtDef.StatNames id, MinionType minionType)
    {
        var result = -1f;
        switch (id)
        {
            case MinionBoughtDef.StatNames.HP:
                result = hp;
                break;
            case MinionBoughtDef.StatNames.SPD:
                result = speed;
                break;
            case MinionBoughtDef.StatNames.PASSIVE:
                if (minionType == MinionType.Healer)
                    result = healPerSecond;
                else if (minionType == MinionType.WarScreamer)
                    result = passiveSpeedDelta;

                break;
            case MinionBoughtDef.StatNames.SKILL:

                switch (minionType)
                {
                    case MinionType.Runner:
                        result = skillDeltaSpeed;
                        break;
                    case MinionType.Tank:
                        result = skillArea;
                        break;
                    case MinionType.Healer:
                        result = skillHealAmount;
                        break;
                    case MinionType.Zeppelin:
                        result = miniZeppelinStat.hitsToDie;
                        break;
                    case MinionType.WarScreamer:
                        result = activeSpeedDelta;
                        break;
                }
                break;
        }

        return result;
    }
}
