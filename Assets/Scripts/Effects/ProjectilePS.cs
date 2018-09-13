using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a particle system that will go from A to B.
/// </summary>
public class ProjectilePS : MonoBehaviour {

    Transform _from;
    Transform _to;
    ParticleSystem _ps;
    Vector3 _dir;
    float _speed;
    void Awake() {
        _ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, 3);
    }

    public void Init(Transform from, Transform to, float speed = 11)
    {
        transform.position = from.position;
        _ps.Play();
        _from = from;
        _to = to;
        _speed = speed;
    }

    void Update ()
    {
        if (_to != null)
        {
            transform.position = Vector3.Lerp(transform.position, _to.position, _speed * Time.deltaTime);
            if(_from != null)
                _dir = (_to.position - _from.position).normalized;
        }
        else
            transform.position += _dir * _speed * Time.deltaTime;

        if (_to != null )
            if (Vector3.Distance(transform.position, _to.position) < 0.5f)
                Destroy(gameObject);
	}
}
