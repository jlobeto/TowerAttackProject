using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public GameObjectTypes type = GameObjectTypes.None;
    public float speed = 6;
    public float damage = 5;
    public float range = 0;

    protected bool pCanMove;
    protected GameObject pTarget;
    
    public void Init(GameObject t)
    {
        pCanMove = true;
        pTarget = t;
    }

    protected virtual void Movement()
    {
        if (!pCanMove) return;

        if(pTarget != null)
            transform.forward = (pTarget.transform.position - transform.position).normalized;
        
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    protected virtual void Start () {
        Destroy(gameObject, 5f);
	}
	
	void Update ()
    {
        Movement();
	}

    protected virtual void DoDamage(Minion m)
    {
        m.GetDamage(damage);
    }

    /// <summary>
    /// Ones the projectile reaches the target. It would be nice if here is all the implementations 
    /// of particles, destroying, etc.
    /// </summary>
    protected virtual void OnTargetReached()
    {
        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Minion"))
        {
            DoDamage(other.GetComponent<Minion>());
            OnTargetReached();
        }
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        if(pCanMove && pTarget != null)
            Gizmos.DrawWireSphere(pTarget.transform.position, range == 0f ? 0.6f : range);*/
    }
}
