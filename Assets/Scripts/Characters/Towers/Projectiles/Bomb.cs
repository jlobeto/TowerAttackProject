using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ProjectileBase
{

    protected override void Start()
    {
        //dont distroy GO till arrives to end point
    }

    /*protected override void Movement()
    {
        if (!pCanMove) return;

        base.Movement();
        if (pTarget != null)
        {
            var dist = Vector3.Distance(transform.position, pTarget.transform.position);
            if (dist < 0.5f)
            {
                pCanMove = false;
                Explode();
            }
        }

    }*/

    protected override void DoDamage(Minion minion)
    {
        var minions = Physics.OverlapSphere(transform.position, range, 1 << LayerMask.NameToLayer("Minion"));
        foreach (var item in minions)
        {
            var m = item.GetComponent<Minion>();

            if (m != null)
                base.DoDamage(m);
        }
    }
    
    void Explode()
    {
        var minions = Physics.OverlapSphere(transform.position, range, 1 << LayerMask.NameToLayer("Minion"));
        foreach (var item in minions)
        {
            var m = item.GetComponent<Minion>();
            
            if (m != null)
                DoDamage(m);
        }
        var p = Instantiate(explotion, transform);
        Destroy(GetComponentInChildren<MeshRenderer>());
        Destroy(p.gameObject, p.main.duration);
        Destroy(gameObject, p.main.duration);
        p.Play();
        
    }

}
