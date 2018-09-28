using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniZeppelin : Minion
{
    public float airYpos = 7;
    public override void InitMinion(WalkNode n, Vector3 pos = default(Vector3))
    {
        hasBeenFreed = true;
        transform.position = new Vector3(pos.x, airYpos, pos.z);
        //the zeppelin will give the next node.
        pNextNode = n;
    }

    public override void GetDamage(float dmg)
    {
        if (IsDead) return;

        if (HasShieldBuff()) return;

        dmg = 1;
        if (pDamageDebuff)
            dmg++;

        hp -= dmg;
        infoCanvas.UpdateLife(hp);
        //CheckPSExplotion();
        //DeathChecker();
        if (IsDead)
        {
            if (infoCanvas != null)
                Destroy(infoCanvas.gameObject);
            GetComponent<Collider>().enabled = false;
            pCanWalk = false;
            OnDeath(this);
        }
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
