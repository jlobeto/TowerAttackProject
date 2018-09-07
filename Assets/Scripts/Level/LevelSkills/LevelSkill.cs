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

    public Action OnSkillCancel = delegate { };

    bool _initialized;
    Vector3 _target;
    GameObject _sphere;
    bool _canCast;

    public void OnInitCast()
    {
        _initialized = true;
        var spot = Resources.Load("Level/Skills/Spotlight", typeof(GameObject)) as GameObject;
        _sphere = Instantiate<GameObject>(spot);
        stats.areaOfEffect = _sphere.GetComponentInChildren<Transform>().localScale.x * 8;
    }

    public void OnCancelCast()
    {
        _initialized = _canCast = false;
        OnSkillCancel();
        Destroy(_sphere);
    }

    void Start () {
		
	}
	
	void Update ()
    {
        if (_initialized)
        {
            _target = GetTarget();
            _sphere.transform.position = new Vector3(_target.x, 13, _target.z);
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
            var go = Physics.OverlapSphere(_target, stats.areaOfEffect).Select(i => i.gameObject).ToList();
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
            
            OnCancelCast();
        }
    }

    Vector3 GetTarget()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = Camera.main.transform.position.y;
        pos = Camera.main.ScreenToWorldPoint(pos);
        var ray = new Ray(pos, (pos - Camera.main.transform.position).normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, 1 << LayerMask.NameToLayer("LevelSkillFloor")))
        {
            var ret = new Vector3(hit.point.x, 1.5f, hit.point.z);
            return hit.point;
        }
        else
            return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (_initialized)
        {
            Gizmos.DrawWireSphere(GetTarget(), stats.areaOfEffect);
        }
    }
}
