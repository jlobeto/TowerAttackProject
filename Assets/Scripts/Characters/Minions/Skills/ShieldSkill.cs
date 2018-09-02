using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : BaseMinionSkill
{
    //Minion _thisMinion;
    int _hitsLeft;
    
    public override bool Initialize(float lastingTime, float cooldown, int times)
    {
        //Debug.Log("shield skill Initialize()");
        var result = base.Initialize(lastingTime, cooldown);

        if (!result) return false;

        //Debug.Log("activate!!");
        infoCanvas.InitShield(times);
        _hitsLeft = times;

        return true;
    }

    /// <summary>
    /// Return true if can continue executing the skill
    /// </summary>
    public override bool ExecuteSkill()
    {
        if (!pIsActivated) return false;

        _hitsLeft--;
        infoCanvas.RemoveShieldHit();
        var continueActivated = _hitsLeft >= 0;
        if (!continueActivated) //if the shield has gone for num of hits, turn 0 the skillTime
            pSkillTime = 0;
        return continueActivated;
    }

    void Start ()
    {
        skillType = SkillType.HitShield;
        //_thisMinion = GetComponent<Minion>();
    }
	
}
