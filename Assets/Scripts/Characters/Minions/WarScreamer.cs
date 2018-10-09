using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarScreamer : Minion 
{
    [HideInInspector] public float areaOfEffect = 5;
    [HideInInspector] public float lifePercentThresholdToActivatePassive = 25;//will activate the passive skill on the minion when that minin has lower percent of life than this threshold
    [HideInInspector] public float passiveSpeedDelta = 1.5f;

    [HideInInspector] public float activeSpeedDelta = 2f;
    [HideInInspector] public float passiveSkillDurationOnAffectedMinion = 1.5f;
    [HideInInspector] public float timeToPassive = 2f;

	[HideInInspector]
	public MinionManager manager;

	float _timerAux = 1;
	WarScreamSkill _mySkill;

	protected override void Start()
	{
		base.Start();
        _timerAux = timeToPassive;
        _mySkill = gameObject.AddComponent<WarScreamSkill>();        
		skills.Add(_mySkill);
		_mySkill.infoCanvas = infoCanvas;
	}

    public override void ActivateSelfSkill()
    {
        OnSkill(true);
    }


    protected override void PerformAction()
	{
		base.PerformAction();

		PassiveSkill ();
	}

	void PassiveSkill()
	{
		if (manager == null) return;

		_timerAux -= Time.deltaTime;
		if (_timerAux < 0)
		{
			_timerAux = timeToPassive;
            OnSkill();	
		}
	}

    void OnSkill(bool activeSkill = false)
    {
        var nearMinions = manager.GetMinions(GetMinionHandler);
        foreach (var item in nearMinions)
        {
            SetSkillToMinion(item, activeSkill);
        }
    }

	void SetSkillToMinion(Minion m, bool activeSkill = false)
	{
		var warscreamSkill = GetMinionSkillIfPossible(m);

		if (warscreamSkill == null)
		{
			warscreamSkill = m.gameObject.AddComponent<WarScreamSkill>();
			m.skills.Add(warscreamSkill);
			warscreamSkill.infoCanvas = m.infoCanvas;
			warscreamSkill.useCanvas = false;
		}

        if(!activeSkill)
		    warscreamSkill.Initialize(passiveSkillDurationOnAffectedMinion,0 ,passiveSpeedDelta, m.speed);
        else
            warscreamSkill.Initialize(skillTime, 0, activeSpeedDelta, m.speed);
    }

	WarScreamSkill GetMinionSkillIfPossible(Minion m)
	{
		return (WarScreamSkill)BaseMinionSkill.GetSkillByType(BaseMinionSkill.SkillType.WarScreamer, m.skills);
	}

	bool GetMinionHandler(Minion m)
	{
		if (m == this) return false;

		var result =  Mathf.Abs(Vector3.Distance(m.transform.position, transform.position)) <= areaOfEffect;
		var currentPercent = m.hp / m.InitialHP;
		return result && currentPercent <= lifePercentThresholdToActivatePassive;
	}
}
