using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : BaseMinionSkill
{
    int _hitsLeft;
    
    public override bool Initialize(float lastingTime, float cooldown, int times)
    {
        //Debug.Log("shield skill Initialize()");
        var result = base.Initialize(lastingTime, cooldown);

        if (!result) return false;

        //Debug.Log("activate!!");
        infoCanvas.InitShield(times);
        _hitsLeft = times;
        pThisMinion.ShieldBubble.SetActive(true);
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
        var continueActivated = _hitsLeft > 0;
        if (!continueActivated) //if the shield has gone for num of hits, turn -1 the skillTime
            pSkillTime = -1;

        return true;
    }

    protected override void OnFinishSkillByTime()
    {
        pThisMinion.ShieldBubble.SetActive(false);
    }

    void Start ()
    {
        skillType = SkillType.HitShield;
        pThisMinion = GetComponent<Minion>();
    }

}
