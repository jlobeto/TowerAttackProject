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

    int _hitsLeft;
    float _skillTimeAux;
    Minion[] _skillAffectedMinions;

    /// <summary>
    /// return true if can receive damage;
    /// </summary>
    bool OnProcessDamage(int hits)
    {
        return hits < 0;
    }

    /// <summary>
    /// This must to be here because is the source of the skill, if hits are wasted, the skill is done.
    /// In the other hand(minions affected with this skill), they does not have any self skill activated, 
    /// the only thing the know is that they have a 'x' number of shield hits to cover out.
    /// </summary>
    public override void GetDamage(float dmg)
    {
        if (pMakeSkill)
            _hitsLeft--;
            
        if (!pMakeSkill || _hitsLeft < 0)
        {
            if (_hitsLeft < 0)
                pMakeSkill = false;
                
            base.GetDamage(dmg);
        }
    }

    public override void ActivateSkill()
    {
        if (pMakeSkill) return;
        
        pMakeSkill = true;
        _skillTimeAux = skillTime;
        _hitsLeft = shieldHits;
        _skillAffectedMinions = GetNearMinions();
        if (_skillAffectedMinions == null || _skillAffectedMinions.Length == 0) return;

        foreach (var item in _skillAffectedMinions)
            SetMinionsVars(item,true);
    }

    protected override void ExecuteSkill()
    {
        if (!pMakeSkill) return;

        _skillTimeAux -= Time.deltaTime;
        if (_skillTimeAux < 0)
        {
            pMakeSkill = false;

            if (_skillAffectedMinions != null)
            {
                foreach (var item in _skillAffectedMinions)
                    SetMinionsVars(item, false);
            }
        }
    }

    /// <summary>
    /// For the skill use. MUST delete this tank from the calculation
    /// </summary>
    Minion[] GetNearMinions()
    {
        var minions = Physics.OverlapSphere(transform.position, skillArea, 1 << LayerMask.NameToLayer("Minion"));

        var arr = minions.Select(i => i.GetComponent<Minion>()).Where(m => m != this).ToArray();
        return arr;
    }

    void SetMinionsVars(Minion m, bool activate)
    {
        if (activate)
        {
            m.tankShieldHits = shieldHits;
            m.OnTankSkill = OnProcessDamage;
        }
        else
        {
            m.tankShieldHits = -1;
            m.OnTankSkill = null;
        }
    }

    private void OnDrawGizmos()
    {
        if(showAreaGizmo)
            Gizmos.DrawWireSphere(transform.position, skillArea);

        if (_skillAffectedMinions != null && pMakeSkill)
        {
            Gizmos.color = Color.blue;
            foreach (var item in _skillAffectedMinions)
            {
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }
}
