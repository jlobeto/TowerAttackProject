using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkNode : MonoBehaviour
{

    public bool isInitial;
    public bool isEnd;
    public List<WalkNode> nexts = new List<WalkNode>();


    bool _alreadyTransited;
	
	void Start () {
		
	}

    public WalkNode GetNextWalkNode()
    {
        var random = Random.Range(0, nexts.Count);
        return nexts[random];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        if(isInitial)
            Gizmos.color = Color.green;
        else if(isEnd)
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, .15f);
    }
}
