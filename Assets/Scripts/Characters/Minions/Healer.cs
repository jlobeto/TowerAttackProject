using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Minion
{
    public float areaOfEffect = 5;
    public float healPerSecond = 2;
    public int skillHealAmount = 15;
    public bool showTestGizmo = true;
    public ProjectilePS givePassiveHealth;
    public ProjectilePS giveActiveHealth;

    [HideInInspector]
    public MinionManager manager;

    HealerSkill _mySkill;
    float _timerAux = 1;

    protected override void Start()
    {
        base.Start();
        _mySkill = gameObject.AddComponent<HealerSkill>();
        skills.Add(_mySkill);
        _mySkill.infoCanvas = infoCanvas;
    }

    public override void ActivateSelfSkill()
    {
        if (_mySkill.IsLocked) return;//check this so we don't run GetMinions if we don't have to.

        var nearMinions = manager.GetMinions(GetMinionHandler);

        _mySkill.InitializeHealerSkill(nearMinions, skillCooldown, giveActiveHealth, skillHealAmount);
    }

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
                var ps = Instantiate<ProjectilePS>(givePassiveHealth);
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
