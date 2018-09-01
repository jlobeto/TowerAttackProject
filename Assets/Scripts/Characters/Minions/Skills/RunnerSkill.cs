using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerSkill : BaseMinionSkill
{
    Minion _myMinion;
    float _lastSpeed;
    bool _isInitialized;

    public override bool Initialize(float lastingTime, float speedDelta, float prevSpeed)
    {
        var result = base.Initialize(lastingTime);

        if (!result) return false;

        _isInitialized = true;
        _myMinion.speed *= speedDelta;
        _lastSpeed = prevSpeed;

        return true;
    }

    public override bool ExecuteSkill()
    {
        if (!pIsEnabled && _isInitialized)
        {
            _myMinion.speed = _lastSpeed;
            _isInitialized = false;
        }

        return pIsEnabled;
    }

    void Start ()
    {
        skillType = SkillType.SpeedBoost;
        _myMinion = GetComponent<Minion>();
    }
	
	

}
