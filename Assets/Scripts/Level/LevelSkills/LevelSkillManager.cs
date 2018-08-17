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

    List<SkillType> _levelSkills;
    

	void Start ()
    {
        Init();
    }
	
	
	void Update () {
		
	}

    void Init()
    {
        _levelSkills = level.levelSkills;
    }

}
