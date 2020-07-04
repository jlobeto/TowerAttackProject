using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Zeppelin : AirMinion
{
    public MiniZeppelin miniZeppelin;

    [HideInInspector]
    public MinionManager manager;
    /// <summary>
    /// When zep is dead, it will spawn this quantity of minizeps
    /// </summary>
    [HideInInspector]
    public int miniZeppelinCount;
    [HideInInspector]
    public int skillMiniZepCount;

    ZeppelinSkill _mySkill;
    ParticleSystem _skillPS;

    protected override void Start()
    {
        base.Start();

        _mySkill = gameObject.AddComponent<ZeppelinSkill>();
        //_skillPS = GetComponentsInChildren<ParticleSystem>().Where(i => i.tag != "MinionDeathParticle").First();
        transform.position = new Vector3(transform.position.x, airYpos, transform.position.z);

        skills.Add(_mySkill);
        _mySkill.infoCanvas = infoCanvas;
        pMainSkill = _mySkill;
    }
    public override void InitMinion(WalkNode n, Vector3 pos = default(Vector3))
    {
        hasBeenFreed = true;
        transform.position = new Vector3(n.transform.position.x, airYpos, n.transform.position.z);
        pNextNode = n.GetNextWalkNode();
    }

    public override void GetDamage(float dmg)
    {
        base.GetDamage(dmg);
        if (IsDead)
        {
            SpawnMiniZep(miniZeppelin);
        }
    }

    void SpawnMiniZep(MiniZeppelin prefab)
    {
        Vector3 plus = Vector3.zero;
        int calc = 1;
        for (int i = 1; i <= miniZeppelinCount; i++)
        {
            manager.SpawnMinion(MinionType.MiniZeppelin, transform.position, prefab);
            manager.SetNextMinionFree(pNextNode, transform.position + plus);

            plus = transform.forward * calc * (i+1);
            calc *= -1;
        }
        
    }

    public override void ActivateSelfSkill()
    {
        var wasDisabled = _mySkill.Initialize(0, skillCooldown, skillMiniZepCount, miniZeppelin.gameObject, pNextNode);

        if (!wasDisabled) return;

        OnMinionSkill(minionType);

        _mySkill.ExecuteSkill();
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
