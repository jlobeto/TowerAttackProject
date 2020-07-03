using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarScreamer : AirMinion
{
    [HideInInspector] public float areaOfEffect = 5;
    [HideInInspector] public float activeSpeedDelta = 2f;

	[HideInInspector]
	public MinionManager manager;

	//float _timerAux = 1;
	WarScreamSkill _mySkill;
    bool skillPhase;//phaseOne = user has clicked to know the skill range

    protected override void Start()
	{
		base.Start();

        transform.position = new Vector3(transform.position.x, airYpos, transform.position.z);
        //_timerAux = timeToPassive;
        _mySkill = gameObject.AddComponent<WarScreamSkill>();        
		skills.Add(_mySkill);
		_mySkill.infoCanvas = infoCanvas;
        pMainSkill = _mySkill;
    }


    public override void InitMinion(WalkNode n, Vector3 pTransform = default(Vector3))
    {
        base.InitMinion(n, pTransform);

        InitSkillAreaEffect(areaOfEffect);
    }

    public override void ActivateSelfSkill()
    {
        /*if (!skillPhase)
        {
            skillZoneEffect.SetActive(true);
            StartCoroutine(StopSkillZoneShow());
            skillPhase = true;
            return;
        }*/
        OnSkill(true);
    }


    IEnumerator StopSkillZoneShow()
    {
        yield return new WaitForSeconds(1.5f);
        skillZoneEffect.SetActive(false);
        skillPhase = false;
    }

    protected override void PerformAction()
	{
		base.PerformAction();

		//PassiveSkill ();
	}

	/*Sacamo la passiva a la mierda
     * void PassiveSkill()
	{
		if (manager == null) return;

		_timerAux -= Time.deltaTime;
		if (_timerAux < 0)
		{
			_timerAux = timeToPassive;
            OnSkill();	
		}
	}*/

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

        /*if(!activeSkill)
		    warscreamSkill.Initialize(passiveSkillDurationOnAffectedMinion,0 ,passiveSpeedDelta, m.speed);
        else*/
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
        return result;// && currentPercent <= lifePercentThresholdToActivatePassive;
	}

    protected override void Walk()
    {

        var nextNodePos = new Vector3(pNextNode.transform.position.x, airYpos, pNextNode.transform.position.z);
        var dir = (nextNodePos - transform.position).normalized;
        transform.forward = dir;
        transform.position += transform.forward * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nextNodePos) <= pDistanceToNextNode)
        {
            if (!pNextNode.isEnd)
                pNextNode = pNextNode.GetNextWalkNode();
            else
                FinishWalk();
        }
    }
}
