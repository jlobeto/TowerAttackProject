using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarScreamSkill : BaseMinionSkill 
{
	float _prevSpeed;

	public override bool Initialize(float lastingTime,float cooldown, float speedDelta, float prevSpeed)
	{
		if (pIsLocked)
			return false;

		if(pThisMinion == null)
			pThisMinion = GetComponent<Minion>();

        if(!pIsActivated)
            pThisMinion.speed *= speedDelta;

        pIsActivated = true;
		pSkillTime = lastingTime;
		pSkillCooldown = cooldown;
		_prevSpeed = prevSpeed;
		return true;
	}


	protected override void OnFinishSkillByTime()
	{
        pThisMinion.speed = _prevSpeed;
        //pThisMinion.EndOfWarScreamerSkill(_prevSpeed);
	}

	protected override void Start()
	{
		base.Start();
		skillType = SkillType.WarScreamer;
	}
}
