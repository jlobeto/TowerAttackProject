using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameplayUtils
{
    public static TargetType GetTargetTypeByMinionType(MinionType type)
    {
        switch (type)
        {
            case MinionType.Runner:
                return TargetType.Ground;
            case MinionType.Tank:
                return TargetType.Ground;
            case MinionType.Dove:
                return TargetType.Air;
            case MinionType.Healer:
                return TargetType.Ground;
            case MinionType.Zeppelin:
                return TargetType.Air;
            case MinionType.MiniZeppelin:
                return TargetType.Air;
            case MinionType.WarScreamer:
                return TargetType.Ground;
        }

        return TargetType.Both;
    }

    public static MinionType GetMinionTypeBySkill(BaseMinionSkill.SkillType type)
    {
        switch (type)
        {
            case BaseMinionSkill.SkillType.None:
                return MinionType.Runner;
            case BaseMinionSkill.SkillType.HitShield:
                return MinionType.Tank;
            case BaseMinionSkill.SkillType.SpeedBoost:
                return MinionType.Runner;
            case BaseMinionSkill.SkillType.GiveHealth:
                return MinionType.Healer;
               
            case BaseMinionSkill.SkillType.ChangeTarget:
                return MinionType.Dove;
               
            case BaseMinionSkill.SkillType.SmokeBomb:
                return MinionType.Zeppelin;

            case BaseMinionSkill.SkillType.WarScreamer:
                return MinionType.WarScreamer;

        }

        return MinionType.Runner;
    }

    public static int StarsWon(int currLives, int[] objetives)
    {
        if (objetives.Length == 0) return 0;

        if (currLives == 0) return 0;

        if (currLives >= objetives[0] && objetives.Length > 1 && currLives < objetives[1]) return 1;
        

        if (objetives.Length > 1 && currLives >= objetives[1] && currLives < objetives[2])
            return 2;

        if (objetives.Length > 2 && currLives >= objetives[2])
            return 3;

        return 0;
    }
}
