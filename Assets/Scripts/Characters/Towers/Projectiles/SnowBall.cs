using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : ProjectileBase
{
    public float freezeTime = 2f;
    public ParticleSystem hitEffect;

    protected override void DoDamage(Minion m)
    {
        base.DoDamage(m);
        m.GetFreezeDebuff(freezeTime);
    }

    protected override void OnTargetReached()
    {
        var ps = Instantiate<ParticleSystem>(hitEffect, transform);
        ps.Play();
        Destroy(GetComponentInChildren<MeshRenderer>());
        Destroy(gameObject, ps.main.duration);
    }
}
