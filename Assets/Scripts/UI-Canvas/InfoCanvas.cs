using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This shit will be inside the gamobject that is above the character head.
/// </summary>
public class InfoCanvas : MonoBehaviour
{
    public Image hpBar;
    public Image skillBar;
    public Image skillBarCooldown;
    public Image shieldSkill;

    float _initLife;
    float _skillTime;
    float _skillCooldown;
    float _shieldMaxHits;
    float _shieldMaxHitsAux;

    public void Init(float life, float skillTime, float skillCooldown)
    {
        _initLife = life;
        _skillTime = skillTime;
        _skillCooldown = skillCooldown;
    }

    public void InitShield(int maxhits)
    {
        _shieldMaxHitsAux = _shieldMaxHits = maxhits;
        shieldSkill.fillAmount = 1;
    }

    public void UpdatePosition(Vector3 pos)
    {
        var newPos = pos;
        newPos.y += 3;
        transform.position = newPos;
    }

    public void UpdateLife(float newLife)
    {
        hpBar.fillAmount = newLife / _initLife;
    }

    public void UpdateSkillTimes(float t, bool skillTime, bool completeFill=false)
    {
        if(skillTime)
            skillBar.fillAmount = !completeFill ? t / _skillTime : 1;
        else
            skillBarCooldown.fillAmount = 1 - (t / _skillCooldown);
    }

    public void RemoveShieldHit()
    {
        _shieldMaxHitsAux--;
        shieldSkill.fillAmount = _shieldMaxHitsAux / _shieldMaxHits;
    }

	void Start () {
		
	}
	
	void Update ()
    {
        var dir = (Camera.main.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(dir * -1);
    }
}
