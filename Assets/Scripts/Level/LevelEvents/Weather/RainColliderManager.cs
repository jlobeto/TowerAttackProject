using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainColliderManager : MonoBehaviour
{
    List<HitAreaCollider> _hitAreaColliders;

    List<Collider> _colliders;

    HitAreaCollider _selected;

    float _speedDelta;

	void Start ()
    {
        _hitAreaColliders = GetComponentsInChildren<HitAreaCollider>().ToList();
        _colliders = GetComponentsInChildren<Collider>().ToList();
        foreach (var item in _hitAreaColliders)
        {
            item.OnTriggerEnterCallback += TriggerEnterHandler;
            item.OnTriggerExitCallback += TriggerExitHandler;
        }

        foreach (var item in _colliders)
        {
            item.enabled = false;
        }
    }

    public HitAreaCollider SelectArea(float speedDelta)
    {
        _selected = _hitAreaColliders[Random.Range(0, _hitAreaColliders.Count)];
        _selected.GetComponent<Collider>().enabled = true;
        _speedDelta = speedDelta;
        return _selected;
    }

    public void StopArea()
    {
        _selected.GetComponent<Collider>().enabled = false;
        _selected = null;
    }

    void TriggerEnterHandler(Collider col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Minion")) return;

        //Debug.Log("TriggerEnterHandler");
        var minion = col.GetComponent<Minion>();
        if (minion == null) return;
        minion.DamageDebuff(true, _speedDelta);
    }
    

    void TriggerExitHandler(Collider col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Minion")) return;
        //Debug.Log("TriggerExitHandler");
        var minion = col.GetComponent<Minion>();
        if (minion == null) return;
        minion.DamageDebuff(false);
    }
}
