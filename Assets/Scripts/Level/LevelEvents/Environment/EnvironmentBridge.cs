using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBridge
{
    public WalkNode pivot;
    public WalkNode destinationA;
    public WalkNode destinationB;
	/// <summary>
	/// this is the minion path (only visual) gameobject.
	/// </summary>
    public EnvironBridgeEffect bridge_A_GameObject;
    public EnvironBridgeEffect bridge_B_GameObject;
    public BridgeClock bridgeClock;//the battery charge clock in scene;
    public bool isPointingA;

    public EnvironmentBridge(WalkNode p , WalkNode a=null, WalkNode b=null)
    {
        pivot = p;
        destinationA = a;
        destinationB = b;
        isPointingA = true;
    }
}
