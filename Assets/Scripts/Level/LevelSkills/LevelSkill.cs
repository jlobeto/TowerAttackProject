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
    GameObject _effectPrefab;
    int _currentUses;
    bool _initialized;
    Vector3 _target;
    GameObject _skillGO;
    bool _canCast;

    public void OnInitCast()
    {
        _initialized = true;
        _skillGO = Instantiate<GameObject>(_spotPrefab);
        var sphere = _skillGO.GetComponentInChildren<MeshRenderer>();
        sphere.transform.localScale = new Vector3(stats.areaOfEffect, stats.areaOfEffect, stats.areaOfEffect);

        var lightSprite = _skillGO.GetComponentInChildren<SpriteRenderer>();
        lightSprite.transform.localScale = new Vector3(stats.areaOfEffect * 2.5f, stats.areaOfEffect * 2.5f, 1);
        
    }

    public void StopCasting()
    {
        _initialized = _canCast = false;
        Destroy(_skillGO);
    }

    void Start () {
        _spotPrefab = Resources.Load("Level/Skills/LevelSkillCoockie"+ stats.skillType, typeof(GameObject)) as GameObject;
        _effectPrefab = Resources.Load("Level/Skills/LvlSkillEffect" + stats.skillType, typeof(GameObject)) as GameObject;
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
            var go = Physics.OverlapSphere(_target, stats.areaOfEffect).Select(i => i.gameObject).ToList();
            List<GameObject> affectedOnes = null;
            switch (skillType)
            {
                case LevelSkillManager.SkillType.Stun:
                    affectedOnes = castSkill.CastSkill(go, stats.effectTime);
                    break;
                case LevelSkillManager.SkillType.Slow:
                    affectedOnes = castSkill.CastSkill(go, stats.fireRate , stats.effectTime);
                    break;
                default:
                    break;
            }

            if (affectedOnes != null)
                SetSkillFeedback(affectedOnes);

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
            return new Vector3( hit.point.x , 0 , hit.point.z );
        else
            return Vector3.zero;
    }

    void SetSkillFeedback(List<GameObject> affectedOnes)
    {
        foreach (var item in affectedOnes)
        {
            var e = Instantiate<GameObject>(_effectPrefab, item.transform);
            e.transform.localPosition = new Vector3(0, 3.5f, 0);
        }
    }


    private void OnDrawGizmos()
    {
        if (_initialized)
        {
            Gizmos.DrawWireSphere(GetTarget(), stats.areaOfEffect);
        }
    }
}
