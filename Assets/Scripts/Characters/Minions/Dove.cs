using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dove : Minion
{
    public float airYpos = 7;
    public float groundYpos = 1;
	public float shadowGroundPos = -0.32f;
	public float shadowAirPos = -2.11f;
	public Transform shadow;

    ParticleSystem _skillPS;
    ChangeTargetSkill _mySkill;

    float _targetPos;
    
    protected override void Start()
    {
        base.Start();
        
        _mySkill = gameObject.AddComponent<ChangeTargetSkill>();
        _skillPS = GetComponentsInChildren<ParticleSystem>().Where(i => i.tag != "MinionDeathParticle").First();
        transform.position = new Vector3(transform.position.x, airYpos, transform.position.z);

        skills.Add(_mySkill);
        _targetPos = airYpos;
        _mySkill.infoCanvas = infoCanvas;
    }

    public override void InitMinion(WalkNode n, Vector3 position=default(Vector3))
    {
        hasBeenFreed = true;
        transform.position = new Vector3( n.transform.position.x, airYpos, n.transform.position.z);
        pNextNode = n.GetNextWalkNode();
    }

    public override void ActivateSelfSkill()
    {
        //Debug.Log("activated");
        var wasDisabled = _mySkill.Initialize(0, skillCooldown);

        if (!wasDisabled) return;

        OnMinionSkill(minionType);

        targetType = targetType == TargetType.Air ? TargetType.Ground : TargetType.Air;
        _targetPos = targetType == TargetType.Air ? airYpos : groundYpos;
		var shadowTargetPos = targetType == TargetType.Air ? shadowAirPos : shadowGroundPos;
		shadow.transform.localPosition = new Vector3 (shadow.transform.localPosition.x, shadowTargetPos ,shadow.transform.localPosition.z);
        _skillPS.Play();
		_mySkill.SetYDest(_targetPos, shadowTargetPos);
    }

    protected override void Walk()
    {
        _mySkill.ExecuteSkill();

        var nextNodePos = new Vector3(pNextNode.transform.position.x, _targetPos, pNextNode.transform.position.z);
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
