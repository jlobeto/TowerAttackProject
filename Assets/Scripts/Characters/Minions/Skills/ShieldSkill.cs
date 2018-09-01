using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : BaseMinionSkill
{
    //Minion _thisMinion;
    int _hitsLeft;
    
    public override bool Initialize(float lastingTime, int times)
    {
        //Debug.Log("shield skill Initialize()");
        var result = base.Initialize(lastingTime);

        if (!result) return false;

        _hitsLeft = times;

        return true;
    }

    /// <summary>
    /// Return true if can continue executing the skill
    /// </summary>
    public override bool ExecuteSkill()
    {
        if (!pIsEnabled) return false;

        _hitsLeft--;
        pIsEnabled = _hitsLeft >= 0;
        return pIsEnabled;
    }

    void Start ()
    {
        skillType = SkillType.HitShield;
        //_thisMinion = GetComponent<Minion>();
    }
	
}
