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
    public InfoCanvas infoCanvas;
    public bool useCanvas = true;//If useCanvas, it will call all the functions to run the infoCanvas and add more feedback

    protected bool pIsActivated;
    protected bool pIsLocked;
    protected float pSkillTime;
    protected float pSkillCooldown;

    protected Minion pThisMinion;

    public bool IsActivated { get { return pIsActivated; } }

    
    public virtual bool Initialize(float time, float cooldown)
    {
        if (pIsLocked)
            return false;

        if (pIsActivated)
        {
            Debug.Log("skill already activated.");
            return false;
        }

        pIsActivated = true;
        pSkillTime = time;
        pSkillCooldown = cooldown;
        if(useCanvas)
            infoCanvas.UpdateSkillTimes(pSkillTime, true);

        return true;
    }

    /// <summary>
    /// in this case times is the number of hits for shield skill
    /// </summary>
    public virtual bool Initialize(float lastingTime, float cooldown, int times)
    {
        return this.Initialize(lastingTime, cooldown);
    }

    /// <summary>
    /// Params for runner skill
    /// </summary>
    public virtual bool Initialize(float lastingTime,float cooldown, float speedDelta, float prevSpeed)
    {
        return this.Initialize(lastingTime, cooldown);
    }


    public virtual bool ExecuteSkill()
    {
        throw new NotImplementedException("ExecuteSkill() must be implemented inside all children");
    }

    void Update()
    {
        if (pIsActivated)
        {
            pSkillTime -= Time.deltaTime;
            if (useCanvas) infoCanvas.UpdateSkillTimes(pSkillTime, true);
            if (pSkillTime < 0)
            {
                if (useCanvas) infoCanvas.UpdateSkillTimes(0, true);
                infoCanvas.shieldSkill.fillAmount = 0;//this one must to update its shieldskill bar.
                pIsActivated = false;
                pIsLocked = true;
                OnFinishSkillByTime();
            }
        }

        if (pIsLocked)
        {
            pSkillCooldown -= Time.deltaTime;
            if (useCanvas) infoCanvas.UpdateSkillTimes(pSkillCooldown, false);
            if (pSkillCooldown < 0)
            {
                pIsLocked = false;
                if (useCanvas) infoCanvas.UpdateSkillTimes(0, false);
                if (useCanvas) infoCanvas.UpdateSkillTimes(pSkillTime, true, true);
            }
        }
    }

    protected virtual void OnFinishSkillByTime()
    {

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
