using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tank : Minion
{
    public int shieldHits = 3;
    public float skillArea = 5;
    public bool showAreaGizmo;

    Minion[] _skillAffectedMinions;
    ShieldSkill _mySkill;
    bool skillPhase;//phaseOne = user has clicked to know the skill range

    protected override void Start()
    {
        base.Start();
        _mySkill = gameObject.AddComponent<ShieldSkill>();
        skills.Add(_mySkill);
        _mySkill.infoCanvas = infoCanvas;
    }

    public override void InitMinion(WalkNode n, Vector3 pTransform = default(Vector3))
    {
        base.InitMinion(n, pTransform);

        InitSkillAreaEffect(skillArea);
    }

    public override void GetDamage(float dmg)
    {
        if (_mySkill == null)
        {
            base.GetDamage(dmg);
            return;
        }

        var isEnabled =_mySkill.ExecuteSkill();

        if (!isEnabled)
        {
            //Debug.Log("Get real Damage");
            base.GetDamage(dmg);
        }
        /*else
            Debug.Log("damage shielded");*/

    }

    public override void ActivateSelfSkill()
    {
        if (!skillPhase)
        {
            skillZoneEffect.SetActive(true);
            StartCoroutine(StopSkillZoneShow());
            skillPhase = true;
            return;
        }

        var wasDisabled =_mySkill.Initialize(skillTime, skillCooldown ,shieldHits);

        //If the skill has been already enabled(because other tank has given it) return and false selfSkillEnabled
        if (!wasDisabled) return;

        _skillAffectedMinions = GetNearMinions();
        if (_skillAffectedMinions.Length == 0) return;

        foreach (var item in _skillAffectedMinions)
            SetSkillToMinion(item,true);
    }

    IEnumerator StopSkillZoneShow()
    {
        yield return new WaitForSeconds(1.5f);
        skillZoneEffect.SetActive(false);
        skillPhase = false;
    }

    /// <summary>
    /// For the skill use. MUST delete this tank from the algorithm
    /// If any selected minions has shield skill activated it won't be included in the resulting array;
    /// </summary>
    Minion[] GetNearMinions()
    {
        var minions = Physics.OverlapSphere(transform.position, skillArea, 1 << LayerMask.NameToLayer("Minion"));
        var arr = minions.Select(i => i.GetComponent<Minion>()).Where(m => m != this && !m.IsDead).ToArray();
        Minion[] result = new Minion[arr.Length];
        int resultIndex = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            var m = arr[i];
            var skill = GetMinionShieldIfPossible(m);
            if (skill == null || !skill.IsActivated)
            {
                result[resultIndex] = m;
                resultIndex++;
            }
        }
        return result.Where(i => i != null).ToArray();
    }

    void SetSkillToMinion(Minion m, bool activate)
    {
        var shieldSkill = GetMinionShieldIfPossible(m);

        if (shieldSkill == null)
        {
            shieldSkill = m.gameObject.AddComponent<ShieldSkill>();
            m.skills.Add(shieldSkill);
            shieldSkill.infoCanvas = m.infoCanvas;
            shieldSkill.useCanvas = false;
        }
        
        shieldSkill.Initialize(skillTime,0 ,shieldHits);
    }

    ShieldSkill GetMinionShieldIfPossible(Minion m)
    {
        return (ShieldSkill)BaseMinionSkill.GetSkillByType(BaseMinionSkill.SkillType.HitShield, m.skills);
    }

    private void OnDrawGizmos()
    {
        if(showAreaGizmo)
            Gizmos.DrawWireSphere(transform.position, skillArea);
        
        if (_skillAffectedMinions != null)
        {
            Gizmos.color = Color.blue;
            foreach (var item in _skillAffectedMinions)
            {
                var skill = GetMinionShieldIfPossible(item);
                if(skill != null && skill.IsActivated)
                    Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }
}
