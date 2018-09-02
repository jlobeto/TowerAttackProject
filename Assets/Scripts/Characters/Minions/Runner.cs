using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Minion
{
    public float skillDeltaSpeed = 2;
    RunnerSkill _mySkill;

    protected override void Start()
    {
        base.Start();
        _mySkill = gameObject.AddComponent<RunnerSkill>();
        skills.Add(_mySkill);
        _mySkill.infoCanvas = infoCanvas;

    }

    public override void GetDamage(float dmg)
    {
        if (!_mySkill.IsActivated)
            base.GetDamage(dmg);
    }

    protected override void Walk()
    {
        pBuffInvisible = _mySkill.ExecuteSkill();
        base.Walk();
    }


    public override void ActivateSelfSkill()
    {
        _mySkill.Initialize(skillTime, skillCooldown ,skillDeltaSpeed, speed);
    }
}
