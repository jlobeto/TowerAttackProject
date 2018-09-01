using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTargetSkill : BaseMinionSkill
{
    float _to;
    void Start()
    {
        skillType = SkillType.SpeedBoost;
    }

    public void SetYDest(float y)
    {
        _to = y;
    }

    public override bool ExecuteSkill()
    {
        if (_to == 0) return false;

        var y = transform.position.y;
        y = Mathf.Lerp(y, _to, Time.deltaTime * 3);
        
        /*if (y >= _to - 0.2f || y <= _to + 0.2f)
        {
            y = _to;
            _to = 0;
        }*/
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        return true;
    }

}
