using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerSkill : BaseMinionSkill
{
    float _lastSpeed;
    bool _isInitialized;

    public override bool Initialize(float lastingTime,float cooldown, float speedDelta, float prevSpeed)
    {
        var result = base.Initialize(lastingTime, cooldown);

        if (!result) return false;

        _isInitialized = true;
        pThisMinion.speed *= speedDelta;
        _lastSpeed = prevSpeed;

        return true;
    }

    public override bool ExecuteSkill()
    {
        if (!pIsActivated && _isInitialized)
        {
            pThisMinion.speed = _lastSpeed;
            _isInitialized = false;
        }

        return pIsActivated;
    }

    protected override void Start()
    {
        base.Start();
        skillType = SkillType.SpeedBoost;
    }

}
