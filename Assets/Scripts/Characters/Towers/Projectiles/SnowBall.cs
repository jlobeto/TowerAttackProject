using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : ProjectileBase
{
    float _freezeTime = 1f;
    //public ParticleSystem hitEffect;

    public void Init(GameObject target, float dmg, float rng, float freezeTime, TargetType tt)
    {
        base.Init(target, dmg, rng, tt);
        _freezeTime = freezeTime;
    }

    protected override void DoDamage(Minion m)
    {
        base.DoDamage(m);
        m.GetFreezeDebuff(_freezeTime);
    }

    /*protected override void OnTargetReached()
    {
        var ps = Instantiate<ParticleSystem>(hitEffect, transform);
        ps.Play();
        Destroy(GetComponentInChildren<MeshRenderer>());
        Destroy(gameObject, ps.main.duration);
    }*/
}
