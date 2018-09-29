using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBridge
{
    public WalkNode pivot;
    public WalkNode destinationA;
    public WalkNode destinationB;
    public EnvironBridgeEffect bridge_A_GameObject;
    public EnvironBridgeEffect bridge_B_GameObject;
    public bool isPointingA;

    public EnvironmentBridge(WalkNode p , WalkNode a=null, WalkNode b=null)
    {
        pivot = p;
        destinationA = a;
        destinationB = b;
        isPointingA = true;
    }
}
