using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUtils
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
}
