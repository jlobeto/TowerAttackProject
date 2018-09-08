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
    void Awake() {
        _ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, 5);
    }

    public void Init(Transform from, Transform to)
    {
        transform.position = from.position;
        _ps.Play();
        _from = from;
        _to = to;
    }

    void Update ()
    {
        if(_to != null && _from != null)
            _dir = (_to.position - _from.position).normalized;

        Debug.Log(_to.position);
        transform.position = Vector3.Lerp(transform.position, _to.position, 10 * Time.deltaTime); 

        if (_to != null )
            if (Vector3.Distance(transform.position, _to.position) < 0.5f)
                Destroy(gameObject);
	}
}
