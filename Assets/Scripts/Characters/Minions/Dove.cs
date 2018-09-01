using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dove : Minion
{
    public float airYpos = 2.3f;
    public float groundYpos = 0.5f;

    float _targetPos;
    ChangeTargetSkill _mySkill;

    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(transform.position.x, airYpos, transform.position.z);
        _mySkill = gameObject.AddComponent<ChangeTargetSkill>();
        skills.Add(_mySkill);
        _targetPos = airYpos;
    }

    public override void InitMinion(WalkNode n)
    {
        transform.position = new Vector3( n.transform.position.x, airYpos, n.transform.position.z);
        pNextNode = n.GetNextWalkNode();
    }

    public override void ActivateSelfSkill()
    {
        Debug.Log("activated");
        var wasDisabled = _mySkill.Initialize();

        if (!wasDisabled) return;

        targetType = targetType == TargetType.Air ? TargetType.Ground : TargetType.Air;
        _targetPos = targetType == TargetType.Air ? airYpos : groundYpos;

        _mySkill.SetYDest(_targetPos);
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
