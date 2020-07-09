using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : GroundMinion
{
    public float skillDeltaSpeed = 2;
    RunnerSkill _mySkill;

    protected override void Start()
    {
        base.Start();
        _mySkill = gameObject.AddComponent<RunnerSkill>();
        skills.Add(_mySkill);
        _mySkill.infoCanvas = infoCanvas;
        pMainSkill = _mySkill;
    }

    public override void GetDamage(float dmg)
    {
        if (_mySkill == null || !_mySkill.IsActivated)
            base.GetDamage(dmg);
    }

    public override void GetFreezeDebuff(float t)
    {
        if(!_mySkill.IsActivated)
            base.GetFreezeDebuff(t);
    }

    protected override void Walk()
    {
        pBuffInvisible = _mySkill.ExecuteSkill();
        base.Walk();
    }


    public override void ActivateSelfSkill()
    {
        var result = _mySkill.Initialize(skillTime, skillCooldown ,skillDeltaSpeed, speed);
        if (result)
            OnMinionSkill(minionType);
    }
}
