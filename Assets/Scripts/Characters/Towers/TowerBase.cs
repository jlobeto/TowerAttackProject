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
	public SpriteRenderer attackRangeSprite;
    public Transform rangeFeedbackPosition;//position where the range of the tower will spawn;
    public ParticleSystem stunEffectPS;
    public bool showGizmoRange;
    public float testRange = 5f;

    protected bool isInitialized;
    public TowerStat pMyStat;
    protected bool pImStunned;
    protected bool pSlowDebuff;
    protected GameObject pTarget;
    protected GameObject pLastTarget;
    protected bool pIsTutoLevelCero;

    bool _hasSpawnFirstBullet;
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

        if(pIsTutoLevelCero && !_hasSpawnFirstBullet)
        {
            GetTarget();
            //SpawnProjectile();
        }
    }

    protected virtual void SpawnProjectile()
    {
        if (pTarget == null) return;

        var random = projectilePrefabs[Random.Range(0, projectilePrefabs.Count)];
        var p = Instantiate(random, spawnPoint.transform.position, spawnPoint.transform.rotation);
        p.Init(pTarget, pMyStat.bulletDamage, pMyStat.bulletRange, targetType);

        _hasSpawnFirstBullet = true;
    }

    protected virtual void GetTarget()
    {
        List<Collider> allMinions = new List<Collider>();

        if (rangeFeedbackPosition != null)
        {
            var minions = Physics.OverlapSphere(rangeFeedbackPosition.position, pMyStat.fireRange, 1 << LayerMask.NameToLayer("Minion"));
            allMinions.AddRange(minions);
        }

        if (allMinions.Count == 0)
        {
            allMinions = Physics.OverlapSphere(transform.position, pMyStat.fireRange, 1 << LayerMask.NameToLayer("Minion")).ToList();
            if (allMinions.Count == 0)
            {
                pTarget = null;
                return;
            }
        }

        var minDist = float.MaxValue;
        foreach (var item in allMinions.Select(i => i.GetComponent<Minion>()).Where(i => i.IsTargetable))
        {
            if (targetType != TargetType.Both)
                if (item.targetType != targetType) continue;

            var dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                pTarget = item.gameObject;
            }
        }

        if (pTarget != null && pLastTarget != null && pLastTarget != pTarget)
            OnTargetHasChanged();

        pLastTarget = pTarget;
    }

    protected virtual void OnTargetHasChanged()
    {

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

        if (stunEffectPS != null)
        {
            stunEffectPS.Play(true);
        }

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
        if (stunEffectPS != null)
            stunEffectPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
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

    public void ActivateAttackRangeSprite()
    {
        if(attackRangeSprite == null)
        {
            //throw new System.Exception("There isn't a attackRange particle system on this tower");
			return;
        }
		if (attackRangeSprite.enabled) return;

		attackRangeSprite.enabled = true;
        StartCoroutine(StopShowingAttackRangeSprite());
        StartCoroutine(SpinAttackRange());
    }

    IEnumerator StopShowingAttackRangeSprite()
    {
        yield return new WaitForSeconds(2f);

		attackRangeSprite.enabled = false;
        StopCoroutine(StopShowingAttackRangeSprite());
    }

    IEnumerator SpinAttackRange()
    {
        while(attackRangeSprite.enabled)
        {
            attackRangeSprite.transform.Rotate(0, 0, 0.3f);
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(SpinAttackRange());
    }

    protected virtual void Start()
    {
        _id = gameObject.GetInstanceID();

        if(stunEffectPS !=null)
        {
            stunEffectPS = Instantiate<ParticleSystem>(stunEffectPS, transform);
            stunEffectPS.transform.localPosition = new Vector3(0, 3.5f, 0);
        }
        
    }

    public virtual void Initialize(TowerStat stat, bool tutoLevelCero = false)
    {
        isInitialized = true;
        pMyStat = stat;
        _fireRateAux = pMyStat.fireCooldown;
        InitAtkRangeSprite();
        pIsTutoLevelCero = tutoLevelCero;
    }

    /// <summary>
    /// Will set the fireRange to the particles system
    /// </summary>
    void InitAtkRangeSprite()
    {
        if (attackRangeSprite != null)
        {
			attackRangeSprite.drawMode = SpriteDrawMode.Sliced;
			attackRangeSprite.size = new Vector2 (pMyStat.fireRange * 2, pMyStat.fireRange * 2);
			attackRangeSprite.enabled = false;
			attackRangeSprite.transform.position = rangeFeedbackPosition.position;
        }
    }

    protected virtual void Update()
    {
		if (!isInitialized)
			return;
		
        if (!pImStunned)
        {
            Fire();
            MinionAiming();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (showGizmoRange)
        {
            if (rangeFeedbackPosition != null)
                Gizmos.DrawWireSphere(rangeFeedbackPosition.position, testRange);

        }
    }
}
