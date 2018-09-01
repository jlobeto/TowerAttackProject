using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMinionSkill : MonoBehaviour
{
    public enum SkillType
    {
        None,
        HitShield, //Shield that last for a number of hits.
        SpeedBoost,
        ChangeTarget //Change targets types (ground and air) when is needed.
    }

    public SkillType skillType = SkillType.None;
    
    protected bool pIsEnabled;
    protected float pSkillTime;

    public bool IsEnabled { get { return pIsEnabled; } }

    public virtual bool Initialize()
    {
        if (pIsEnabled)
        {
            Debug.Log("skill already activated.");
            return false;
        }

        pIsEnabled = true;
        return true;
    }

    public virtual bool Initialize(float time)
    {
        if (pIsEnabled)
        {
            Debug.Log("skill already activated.");
            return false;
        }

        pIsEnabled = true;
        pSkillTime = time;

        return true;
    }

    /// <summary>
    /// in this case times is the number of hits for shield skill
    /// </summary>
    public virtual bool Initialize(float lastingTime, int times)
    {
        return this.Initialize(lastingTime);
    }

    /// <summary>
    /// Params for runner skill
    /// </summary>
    public virtual bool Initialize(float lastingTime, float speedDelta, float prevSpeed)
    {
        return this.Initialize(lastingTime);
    }


    public virtual bool ExecuteSkill()
    {
        throw new NotImplementedException("ExecuteSkill() must be implemented inside all children");
    }

    void Update()
    {
        if (pIsEnabled)
        {
            pSkillTime -= Time.deltaTime;
            if (pSkillTime < 0)
            {
                pIsEnabled = false;
            }
        }
    }


    public static BaseMinionSkill GetSkillByType(SkillType type, List<BaseMinionSkill> minionSkills)
    {
        foreach (var item in minionSkills)
        {
            if (item.skillType == type)
                return item;
        }

        return null;
    }
}
