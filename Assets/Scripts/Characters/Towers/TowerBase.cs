using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TowerBase : MonoBehaviour
{
    public string towerName;
    public TowerType towerType;
    
    public GameObjectTypes objectType = GameObjectTypes.None;
    public TargetType targetType = TargetType.Both;
    public List<ProjectileBase> projectilePrefabs = new List<ProjectileBase>();
        
    public Transform spawnPoint;
    public GameObject toRotate;
    public bool showGizmoRange;

	protected bool isInitialized;
	public TowerStat pMyStat;
    protected bool pImStunned;
    protected bool pSlowDebuff;
    protected GameObject pTarget;

    float _fireRateAux;
    int _id;

    public int Id { get { return _id; } }


    #region Attack
    protected virtual void Fire()
    {
        _fireRateAux -= Time.deltaTime;
        if (_fireRateAux < 0)
        {
			_fireRateAux = pMyStat.fireCooldown;
            GetTarget();
            SpawnProjectile();
        }
    }

    protected virtual void SpawnProjectile()
    {
        if (pTarget == null) return;

        var random = projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
        var p = Instantiate(random, spawnPoint.transform.position, spawnPoint.transform.rotation);
		p.Init(pTarget, pMyStat.bulletDamage, pMyStat.bulletRange, targetType);
    }

    protected virtual void GetTarget()
    {
		var minions = Physics.OverlapSphere(transform.position, pMyStat.fireRange, 1 << LayerMask.NameToLayer("Minion"));
        if (minions.Length == 0)
        {
            pTarget = null;
            return;
        }

        var minDist = float.MaxValue;
        foreach (var item in minions.Select(i => i.GetComponent<Minion>()).Where(i => i.IsTargetable))
        {
            if(targetType != TargetType.Both)
                if (item.targetType != targetType) continue;

            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                pTarget = item.gameObject;
            }
        }
    }

    protected virtual void MinionAiming()
    {
        if (pTarget == null) return;

        Vector3 dir = pTarget.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(toRotate.transform.rotation, lookRotation, Time.deltaTime * pMyStat.rotationSpeed).eulerAngles;
        toRotate.transform.rotation = Quaternion.Euler(0f, rotation.y, 0);
    }

    #endregion

    #region Debuffs
    public virtual void ReceiveStun(float time)
    {
        pImStunned = true;
        StartCoroutine(StoppingStunDebuff(time));
    }

    public void SlowDebuff(float time, float deltaFireRate)
    {
        _fireRateAux *= deltaFireRate;
        pSlowDebuff = true;
        StartCoroutine(StoppingSlowDebuff(time));
    }

    protected virtual IEnumerator StoppingStunDebuff(float time)
    {
        yield return new WaitForSeconds(time);

        pImStunned = false;
        //visual effect for feedback
        var effect = GetComponentsInChildren<ParticleSystem>().FirstOrDefault(i => i.tag == "LevelSkillEffect");
        if (effect != null)
            Destroy(effect.gameObject);
    }

    IEnumerator StoppingSlowDebuff(float time)
    {
        yield return new WaitForSeconds(time);

        pSlowDebuff = false;
		_fireRateAux = pMyStat.fireCooldown;
        //visual effect for feedback
        var effect = GetComponentsInChildren<ParticleSystem>().FirstOrDefault(i => i.tag == "LevelSkillEffect");
        if (effect != null)
            Destroy(effect.gameObject);
    }
    #endregion

    protected virtual void Start()
    {
        _id = gameObject.GetInstanceID();
        
    }

	public virtual void Initialize(TowerStat stat)
	{
		isInitialized = true;
		pMyStat = stat;
		_fireRateAux = pMyStat.fireCooldown;
	}

    protected virtual void Update()
    {
		if (!isInitialized)
			return;
		
        if (!pImStunned)
        {
            MinionAiming();
            Fire();
        }   
    }

    private void OnDrawGizmos()
    {
		if (!isInitialized)
			return;
		
        Gizmos.color = Color.black;
        if(showGizmoRange)
			Gizmos.DrawWireSphere(transform.position, pMyStat.fireRange);
    }
}
