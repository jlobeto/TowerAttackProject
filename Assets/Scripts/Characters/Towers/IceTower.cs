using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : TowerBase
{
    [Range(0f, 1f)]
    public float deltaSpeed = 0.4f;

    HitAreaCollider _hitAreaCollider;
    ParticleSystem _particleSys;
    //List<Minion> _affectedMinions = new List<Minion>();//minions affected by the ice of the tower

	void Start () {
        _hitAreaCollider = GetComponentInChildren<HitAreaCollider>();
        _hitAreaCollider.OnTriggerStayCallback += OnTriggerStayHandler;
        _hitAreaCollider.OnTriggerExitCallback += OnTriggerExitHandler;
        _hitAreaCollider.OnTriggerEnterCallback += OnTriggerEnterHandler;

        _particleSys = GetComponentInChildren<ParticleSystem>();
    }
	
	
	protected override void Update ()
    {
        if (pImStunned)
        {
            StunTimer();
        }

        if (pSlowDebuff)
            SlowTimer();
    }

    public override void ReceiveStun(float time)
    {
        base.ReceiveStun(time);
        _particleSys.Stop();
    }

    protected override void StunTimer()
    {
        base.StunTimer();

        if(!pImStunned)
            _particleSys.Play();
    }

    void OnTriggerStayHandler(Collider other)
    {

    }

    void OnTriggerEnterHandler(Collider other)
    {
        if (pImStunned) return;

        var m = other.GetComponent<Minion>();
        if (m != null && m.IsTargetable/*&& !_affectedMinions.Contains(m)*/)
        {
            if (m.targetType == targetType || targetType == TargetType.Both)
            {
                //_affectedMinions.Add(m);
                m.GetSlowDebuff(0, deltaSpeed);
            }
        }
    }

    void OnTriggerExitHandler(Collider other)
    {
        var m = other.GetComponent<Minion>();
        if (m != null /*&& _affectedMinions.Contains(m)*/)
        {
            //_affectedMinions.Remove(m);
            if (m.targetType == targetType || targetType == TargetType.Both)
            {
                m.StopSlowDebuff();
            }
        }
            
    }

}
