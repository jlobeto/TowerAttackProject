using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainColliderManager : MonoBehaviour
{
    List<HitAreaCollider> _hitAreaColliders;

    List<Collider> _colliders;
    List<MeshRenderer> _meshModified;

    HitAreaCollider _selected;

    float _speedDelta;

	void Start ()
    {
        _meshModified = new List<MeshRenderer>();
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
        _meshModified = new List<MeshRenderer>();
        _selected = _hitAreaColliders[Random.Range(0, _hitAreaColliders.Count)];
        _selected.GetComponent<Collider>().enabled = true;
        _speedDelta = speedDelta;
        return _selected;
    }

    public void StopArea()
    {
        _selected.GetComponent<Collider>().enabled = false;
        _selected = null;
        foreach (var mesh in _meshModified)
        {
            mesh.enabled = !mesh.enabled;
        }
    }
    
    void TriggerEnterHandler(Collider col)
    {

        if (col.gameObject.layer == LayerMask.NameToLayer("Minion"))
        {
            var minion = col.GetComponent<Minion>();
            if (minion == null) return;
            minion.DamageDebuff(true, _speedDelta);
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("MinionPath"))
        {
            EnableMesh(col);
        }
        //Debug.Log("TriggerEnterHandler");
        
    }

    void EnableMesh(Collider col)
    {
        var mesh = col.GetComponent<MeshRenderer>();
        if (mesh != null)
        {
            mesh.enabled = !mesh.enabled;
            _meshModified.Add(mesh);
        }
    }

    void TriggerExitHandler(Collider col)
    {
        //Debug.Log(col.name);
        if (col.gameObject.layer == LayerMask.NameToLayer("Minion"))
        {
            var minion = col.GetComponent<Minion>();
            if (minion == null) return;
            minion.DamageDebuff(false);
        }
        //Debug.Log("TriggerExitHandler");

    }
}
