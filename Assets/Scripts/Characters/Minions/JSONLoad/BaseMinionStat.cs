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
}
