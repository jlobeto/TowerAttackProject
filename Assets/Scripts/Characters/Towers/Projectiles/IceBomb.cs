using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBomb : Bomb
{
    [Range(0f,1f)]
    public float deltaSpeed = 0.4f;
    public float slowTime = 3f;

    protected override void DoDamage(Minion m)
    {
        base.DoDamage(m);
        m.GetSlowDebuff(slowTime, deltaSpeed);
    }
}
