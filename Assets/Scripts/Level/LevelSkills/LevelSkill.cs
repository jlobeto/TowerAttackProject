using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class LevelSkill : MonoBehaviour
{
    public SkillStats stats;
    public ILevelSkill castSkill;
    public LevelSkillManager.SkillType skillType;

    public Action<int,int, LevelSkillManager.SkillType> OnSkillExecuted = delegate { };

    GameObject _spotPrefab;
    int _currentUses;
    bool _initialized;
    Vector3 _target;
    GameObject _skillGO;
    bool _canCast;

    public void OnInitCast()
    {
        _initialized = true;
        _skillGO = Instantiate<GameObject>(_spotPrefab);
        var sphere = _skillGO.GetComponentInChildren<SphereCollider>();
        sphere.transform.localScale = new Vector3(stats.areaOfEffect, stats.areaOfEffect, stats.areaOfEffect);

        var light = _skillGO.GetComponentInChildren<Light>();
        light.transform.localPosition = new Vector3(0, stats.areaOfEffect / 2, 0);
        light.range = stats.areaOfEffect;
    }

    public void StopCasting()
    {
        _initialized = _canCast = false;
        Destroy(_skillGO);
    }

    void Start () {
        _spotPrefab = Resources.Load("Level/Skills/LevelSkillCoockie"+ stats.skillType, typeof(GameObject)) as GameObject;
    }
	
	void Update ()
    {
        if (_initialized)
        {
            _target = GetTarget();
            _skillGO.transform.position = _target;
            OnCheckInput();
        }
        else
            _target = Vector3.zero;
    }

    void OnCheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _canCast = true;
        }
        else if (Input.GetMouseButtonUp(0) && _canCast)
        {
            ///TODO// Filtrar por gameobjects que interesen(towers, minions y demas) crear alguna layer o tag en comun.
            var go = Physics.OverlapSphere(_target, stats.areaOfEffect/2).Select(i => i.gameObject).ToList();
            switch (skillType)
            {
                case LevelSkillManager.SkillType.Stun:
                    castSkill.CastSkill(go, stats.effectTime);
                    break;
                case LevelSkillManager.SkillType.Slow:
                    castSkill.CastSkill(go, stats.fireRate , stats.effectTime);
                    break;
                default:
                    break;
            }

            _currentUses++;
            OnSkillExecuted(_currentUses,stats.useCountPerLevel, skillType);
            StopCasting();
        }
    }

    Vector3 GetTarget()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit,1000, 1 << LayerMask.NameToLayer("LevelSkillFloor")))
            return hit.point;
        else
            return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (_initialized)
        {
            Gizmos.DrawWireSphere(GetTarget(), stats.areaOfEffect/2);
        }
    }
}
