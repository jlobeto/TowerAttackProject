using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : Minion
{

    public float skillDeltaSpeed = 2;

    float _lastSpeed;//this is the current speed before the skill is triggered.
    float _skillTimeAux;


    public override void GetDamage(float dmg)
    {
        if(!pMakeSkill)//if he is making a skill, no damage will be recieved.
            base.GetDamage(dmg);
    }

    public override void ActivateSkill()
    {
        if (pMakeSkill) return;

        pMakeSkill = true;
        _skillTimeAux = skillTime;
        _lastSpeed = speed;
        speed *= skillDeltaSpeed;
    }

    protected override void ExecuteSkill()
    {
        if (!pMakeSkill) return;

        _skillTimeAux -= Time.deltaTime;
        if (_skillTimeAux < 0)
        {
            pMakeSkill = false;
            speed = _lastSpeed;
            _skillTimeAux = skillTime;
        }
    }
}
