using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeppelinSkill : BaseMinionSkill
{
    GameObject _prefab;
    int _miniZepCount;
    MinionManager _manager;
    WalkNode _nextNode;

    protected override void Start()
    {
        base.Start();
        skillType = SkillType.SmokeBomb;
        _manager = FindObjectOfType<MinionManager>();
    }

    public bool Initialize(float lastingTime, float cooldown, int times, GameObject prefab, WalkNode nextNode)
    {
        
        var result = base.Initialize(lastingTime, cooldown);

        if(!result) return false;

        _miniZepCount = times;
        _prefab = prefab;
        _nextNode = nextNode;
        return true;
    }

    public override bool ExecuteSkill()
    {
        if (!pIsActivated) return false;

        Vector3 plus = Vector3.zero;
        int calc = 1;
        for (int i = 1; i <= _miniZepCount; i++)
        {
            _manager.SpawnMinion(MinionType.MiniZeppelin, transform.position, _prefab.GetComponent<MiniZeppelin>());
            _manager.SetNextMinionFree(_nextNode, transform.position + plus);

            plus = transform.forward * calc * (i + 1);
            calc *= -1;
        }

        return true;
    }
}
