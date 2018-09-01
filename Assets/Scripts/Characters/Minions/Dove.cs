using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dove : Minion
{
    public float yPos = 2.3f;

    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    public override void InitMinion(WalkNode n)
    {
        transform.position = new Vector3( n.transform.position.x, yPos, n.transform.position.z);
        pNextNode = n.GetNextWalkNode();
    }

    protected override void Walk()
    {
        var nextNodePos = new Vector3(pNextNode.transform.position.x, yPos, pNextNode.transform.position.z);
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
