using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public GameObjectTypes type = GameObjectTypes.None;
    public ProjectileTypes projectileType = ProjectileTypes.Ground;
    public float speed = 6;
    public float damage = 5;
    public float range = 0;

    Vector3 _target;
    bool _canMove;
    public void Init(Vector3 t)
    {
        _canMove = true;
        _target = t;
        transform.forward = (_target - transform.position).normalized;
    }

    void Start () {
        Destroy(gameObject, 5f);
	}
	
	void Update ()
    {
        if(_canMove)
            transform.position += transform.forward * speed * Time.deltaTime;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Minion"))
        {
            other.GetComponent<Minion>().GetDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(_canMove)
            Gizmos.DrawWireSphere(_target, range == 0f ? 0.3f : range);
    }
}
