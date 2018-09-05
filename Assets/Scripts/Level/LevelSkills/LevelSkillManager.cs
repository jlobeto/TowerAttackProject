using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSkillManager : MonoBehaviour
{
    public enum SkillType {
        Stun,
        Slow
    }

    public Level level;

    List<SkillType> _levelSkillsTypes;
    List<LevelSkill> _levelSkills;


    void Start ()
    {
        Init();
    }
	
	
	void Update () {
		
	}

    void Init()
    {
        _levelSkillsTypes = level.levelSkills;
        foreach (var item in _levelSkillsTypes)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            var lvlSkill = go.AddComponent<LevelSkill>();
            switch (item)
            {
                case SkillType.Stun:
                    go.name = "Stun";
                    lvlSkill.castSkill = new StunLevelSkill();
                    break;
                case SkillType.Slow:
                    go.name = "Slow";
                    lvlSkill.castSkill = new SlowLevelSkill();
                    break;
            }
            lvlSkill.skillType = item;
            lvlSkill.OnSkillCancel += SkillCancelHandler;
            level.LevelCanvasManager.CreateSkillButton(go.name, lvlSkill.OnInitCast, lvlSkill.OnCancelCast);
        }
    }

    void SkillCancelHandler()
    {
        level.LevelCanvasManager.ActivateSkillButtons();
    }

}
