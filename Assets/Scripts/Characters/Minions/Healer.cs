using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Minion
{
    public float areaOfEffect = 5;
    public float healPerSecond = 2;
    public bool showTestGizmo = true;
    public ProjectilePS giveHealth;

    [HideInInspector]
    public MinionManager manager;

    float _timerAux = 1;

    protected override void PerformAction()
    {
        base.PerformAction();

        HealPerSecond();
    }

    void HealPerSecond()
    {
        if (manager == null) return;

        _timerAux -= Time.deltaTime;
        if (_timerAux < 0)
        {
            _timerAux = 1;
            var nearMinions = manager.GetMinions(GetMinionHandler);
            foreach (var item in nearMinions)
            {
                item.GetHealth(healPerSecond);
                var ps = Instantiate<ProjectilePS>(giveHealth);
                ps.Init(transform, item.transform);
            }
        }
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
