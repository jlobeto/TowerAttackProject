using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public string towerName;
    public float fireRate = 1f;
    public float fireRange = 8f;
    public GameObjectTypes towerType = GameObjectTypes.None;
    public List<ProjectileBase> projectilePrefabs = new List<ProjectileBase>();
    [Tooltip("Tower Level to calculate debuffs numbers, increment damage and so on.")]
    public int level = 1;
    public float rotationSpeed = 5f;
    public Transform spawnPoint;
    public GameObject toRotate;
    public bool showGizmoRange;

    GameObject _target;
    float _fireRateAux;

    protected virtual void SpawnProjectile()
    {
        if (_target == null) return;

        var random = projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
        var p = Instantiate(random, spawnPoint.transform.position, spawnPoint.transform.rotation);
        p.Init(_target.transform.position);
    }

    protected virtual void GetTarget()
    {
        var minions = Physics.OverlapSphere(transform.position, fireRange, 1 << LayerMask.NameToLayer("Minion"));
        if (minions.Length == 0)
        {
            _target = null;
            return;
        }

        var minDist = float.MaxValue;
        foreach (var item in minions)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                _target = item.gameObject;
            }
        }
    }

    protected virtual void MinionAiming()
    {
        if (_target == null) return;

        Vector3 dir = _target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(toRotate.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        toRotate.transform.rotation = Quaternion.Euler(0f, rotation.y, 0);
    }

	void Start ()
    {
        _fireRateAux = fireRate;

    }

    protected virtual void Update ()
    {
        MinionAiming();

        _fireRateAux -= Time.deltaTime;
        if (_fireRateAux < 0)
        {
            _fireRateAux = fireRate;
            GetTarget();
            SpawnProjectile();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if(showGizmoRange)
            Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
