using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSkillManager : MonoBehaviour
{
    public enum SkillType {
        Stun,
        Slow
    }

    public Level level;

    List<SkillType> _levelSkillsTypes;
    SkillStatsList _skillStatsConfig;


    void Start ()
    {
        Init();
    }
	
	
	void Update () {
		
	}

    void Init()
    {
        _skillStatsConfig = GameUtils.LoadConfig<SkillStatsList>("LevelSkillConfig.json");
        //_levelSkillsTypes = level.levelSkills;

        GameObject go = new GameObject();
        go.transform.SetParent(transform);
        go.name = "Level Skills";

        LevelSkill lvlSkill;
        foreach (var skillType in _levelSkillsTypes)
        {
            foreach (var skillData in _skillStatsConfig.skillStatsList)
            {
                if (skillType != GetSkillType(skillData.skillType)) continue;

                lvlSkill = go.AddComponent<LevelSkill>();
                lvlSkill.stats = skillData;
                lvlSkill.castSkill = GetCastSkill(skillType);
                lvlSkill.skillType = skillType;
                lvlSkill.OnSkillExecuted += SkillExecutedHandler;
                /*level.LevelCanvasManager.CreateSkillButton(lvlSkill
                    , lvlSkill.OnInitCast
                    , lvlSkill.StopCasting);*/
            }
        }
    }

    ILevelSkill GetCastSkill(SkillType type)
    {
        switch (type)
        {
            case SkillType.Stun:
                return new StunLevelSkill();
            case SkillType.Slow:
                return new SlowLevelSkill();
        }

        throw new Exception("GetCastSkill() => must update switch with new types");
    }

    SkillType GetSkillType(string type)
    {
        if (Enum.IsDefined(typeof(SkillType), type))
        {
            return (SkillType)Enum.Parse(typeof(SkillType), type);
        }

        throw new Exception("GetSkillType() => type param is '"+ type + "' update LevelSkillManager.SkillType");
    }    

    void SkillExecutedHandler(int currUses, int initUses, SkillType type)
    {
        //level.LevelCanvasManager.SkillExecutedVisualHandler(type, initUses - currUses > 0, currUses, initUses);
    }

}
