using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Minion
{
    public float areaOfEffect = 5;

    public bool showTestGizmo = true;

    [HideInInspector]
    public MinionManager manager;

    protected override void PerformAction()
    {
        base.PerformAction();

        if (manager == null) return;
        //var nearMinions = manager.GetMinions(GetMinionHandler);
        
    }

    bool GetMinionHandler(Minion m)
    {
        if (m == this) return false;

        return Mathf.Abs(Vector3.Distance(m.transform.position, transform.position)) <= areaOfEffect;
    }

    private void OnDrawGizmos()
    {
        if (showTestGizmo)
            Gizmos.DrawWireSphere(transform.position, areaOfEffect);
    }
}
