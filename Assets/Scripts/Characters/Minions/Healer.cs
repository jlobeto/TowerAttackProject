using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : GroundMinion
{
    public float areaOfEffect = 5;
    public float healPerSecond = 2;
    public int skillHealPercent = 15;
    public bool showTestGizmo = true;
    public ProjectilePS givePassiveHealth;
    public ProjectilePS giveActiveHealth;

    HealerSkill _mySkill;
    float _timerAux = 1;
    bool skillPhase;//phaseOne = user has clicked to know the skill range

    protected override void Start()
    {
        base.Start();
        _mySkill = gameObject.AddComponent<HealerSkill>();
        skills.Add(_mySkill);
        _mySkill.infoCanvas = infoCanvas;
        pMainSkill = _mySkill;
    }
    
    public override void InitMinion(WalkNode n, Vector3 pTransform = default(Vector3))
    {
        base.InitMinion(n, pTransform);

        InitSkillAreaEffect(areaOfEffect);
    }

    public override void ActivateSelfSkill()
    {
        /*if (!skillPhase)
        {
            skillZoneEffect.SetActive(true);
            StartCoroutine(StopSkillZoneShow());
            skillPhase = true;
            return;
        }*/

        if (_mySkill.IsLocked) return;//check this so we don't run GetMinions if we don't have to.

        OnMinionSkill(minionType);

        var nearMinions = minionManager.GetMinions(GetMinionHandler);
        _mySkill.InitializeHealerSkill(nearMinions, skillCooldown, giveActiveHealth, skillHealPercent);
    }

    IEnumerator StopSkillZoneShow()
    {
        yield return new WaitForSeconds(1.5f);
        skillZoneEffect.SetActive(false);
        skillPhase = false;
    }

    protected override void PerformAction()
    {
        base.PerformAction();

        HealPerSecond();
    }

    void HealPerSecond()
    {
        if (minionManager == null) return;

        _timerAux -= Time.deltaTime;
        if (_timerAux < 0)
        {
            _timerAux = 1;
            var nearMinions = minionManager.GetMinions(GetMinionHandler);
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
        /*if (showTestGizmo)
            Gizmos.DrawWireSphere(transform.position, areaOfEffect);*/
    }
}
