using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public string minionName;
    [HideInInspector] public float hp = 50;
    public GameObjectTypes type = GameObjectTypes.None;
    public MinionType minionType = MinionType.Runner;
    public TargetType targetType = TargetType.Both;
	public BaseMinionSkill.SkillType skillType;
	public SpriteRenderer skillSelectorSprite;
    [HideInInspector] public float speed = 4;
    [HideInInspector] public int pointsValue = 15;
    [HideInInspector] public float strength = 0;
    [HideInInspector] public float spawnCooldown = 0.5f;
    //public IMinionSkill skill;
    [HideInInspector] public int coinValue = 0;
    [Range(0f,1f)]
    [HideInInspector]
    public float levelPointsToRecover = 0.75f;
    public Action<Minion> OnWalkFinished = delegate { };
    public Action<Minion> OnDeath = delegate { };
    public Action<MinionType> OnMinionSkill = delegate { };
    public InfoCanvas infoCanvas;
    public SimpleParticleSystem sparkParticleSys;
    [HideInInspector]
    public List<BaseMinionSkill> skills = new List<BaseMinionSkill>();
    [HideInInspector] public float skillTime = 2;
    [HideInInspector] public float skillCooldown = 5;
    public bool hasBeenFreed;
    //public ParticleSystem explotion;
    public GameObject skillZoneEffect;

    


    protected bool pDamageDebuff;
    protected float pDamageDebuffValue;
    protected WalkNode pNextNode;
    protected GameObject pShieldBubble;
    protected float pDistanceToNextNode = 0.2f;//To change the next node;
    protected bool pIceDebuff;
    protected bool pBuffInvisible;//like runner run boost skill.
    protected bool pCanWalk;
    protected BaseMinionSkill pMainSkill;

    float _initHP;
    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    float _normalSpeed;
    int _id;
    bool _hasSparkEffect;

    public int Id { get { return _id; } }
    public bool IsTargetable { get { return !pBuffInvisible; } }
    public GameObject ShieldBubble { get { return pShieldBubble; } }
    public bool IsDead { get { return hp <= 0; } }
    public float InitialHP { get { return _initHP; } }
    public BaseMinionSkill MainSkill { get { return pMainSkill; } }

    #region Damage to Minion
    public virtual void GetDamage(float dmg)
    {
        if (IsDead) return;

        if (HasShieldBuff()) return;

        if (pDamageDebuff)
            dmg *= pDamageDebuffValue;

        hp -= dmg;
        infoCanvas.UpdateLife(hp);
        CheckPSSpark();
        DeathChecker();
    }

    /// <summary>
    /// if t == 0, this minion won't manage the timer to remove debuff by it self,
    /// it will wait until another thing gives the call to remove the debuff.
    /// If t != 0, it will use a coroutine to remove debuff.(need to test that because is new)
    /// </summary>
    public void GetSlowDebuff(float t, float speedDelta, bool fromWindDust = false)
    {
        if(fromWindDust)
        {
            if (targetType == TargetType.Ground) return;
        }

        if (!pIceDebuff)
        {
            pIceDebuff = true;
            _normalSpeed = speed;
            speed *= speedDelta;

            if (t != 0)
                StartCoroutine(SlowDebuffTimer(t));
        }
    }

    IEnumerator SlowDebuffTimer(float t)
    {
        yield return new WaitForSeconds(t);
        speed = _normalSpeed;
        pIceDebuff = false;
    }

    public void StopSlowDebuff()
    {
        if (!pIceDebuff) return;

        speed = _normalSpeed;
        pIceDebuff = false;
    }
    
    public virtual void GetFreezeDebuff(float t)
    {
        pCanWalk = false;
        StartCoroutine(FreezeDebuffTimer(t));
    }

    IEnumerator FreezeDebuffTimer(float t)
    {
        yield return new WaitForSeconds(t);
        pCanWalk = true;
    }

    public void DamageDebuff(bool enabled, float dmgDelta = 1)
    {
        pDamageDebuff = enabled;
        pDamageDebuffValue = dmgDelta;
    }
    
    #endregion

    protected bool HasShieldBuff()
    {
        var shieldSkill = (ShieldSkill)BaseMinionSkill.GetSkillByType(BaseMinionSkill.SkillType.HitShield, skills);
        if (shieldSkill != null)
        {
            var enabled = shieldSkill.ExecuteSkill();
            return enabled;
        }

        return false;
    }

    /// <summary>
    /// para testear-
    /// </summary>
    /// <param name="lastSpeed"></param>
    public void EndOfWarScreamerSkill(float lastSpeed)
    {
        if(pIceDebuff)
        {
            speed = lastSpeed;
        }
        else
        {
            speed = _normalSpeed;
        }
    }

    /// <summary>
    /// Increment of HP, if new hp is above initHP the new hp will be initHP
    /// </summary>
    public void GetHealth(float health)
    {
        hp += health;
        if (hp > _initHP)
            hp = _initHP;
    }

    protected virtual void PerformAction()
    {
        if(infoCanvas != null)
            infoCanvas.UpdatePosition(transform.position);

        if (pCanWalk)
        {
            Walk();
        }

    }
    protected virtual void Walk()
    {
        var dir = (pNextNode.transform.position - transform.position).normalized;
        transform.forward = dir;
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, pNextNode.transform.position) <= pDistanceToNextNode)
        {
            if (!pNextNode.isEnd)
                pNextNode = pNextNode.GetNextWalkNode();
            else
                FinishWalk();
        }
    }

    /// <summary>
    /// Set the initial variables values so the skill can be executed.
    /// So maybe here will be all requirement to activate the skill
    /// </summary>
    public virtual void ActivateSelfSkill()
    {
        if (minionType == MinionType.MiniZeppelin) return;

        throw new NotImplementedException("ActivateSkill() is not implemented on dependences.");
    }

    protected void TurnOnSkillRangeEffect()
    {
        
    }

    protected void InitSkillAreaEffect(float skillArea)
    {
        if (skillZoneEffect == null) return;

        
        var sprites = skillZoneEffect.GetComponentsInChildren<SpriteRenderer>();
        skillZoneEffect.transform.localScale = new Vector3(skillArea * 2, skillArea * 2, skillArea * 2);
        foreach (var item in sprites)
        {
            //item.transform.localScale = new Vector3(skillArea * 2, skillArea * 2, skillArea * 2);
        }
    }

    #region Inits, Start and Update
    protected void Init()
    {
        _id = gameObject.GetInstanceID();

        infoCanvas = Instantiate<InfoCanvas>(infoCanvas, transform.position, transform.rotation);
        if(minionType == MinionType.MiniZeppelin)
            infoCanvas.Init(hp, 0,0, true);
        else
            infoCanvas.Init(hp, skillTime, skillCooldown);

        if (minionType == MinionType.Runner)//bugfix of next node when using runner skill.
            pDistanceToNextNode = 0.5f;

        _initHP = hp;
        _normalSpeed = speed;
        if (Debug.isDebugBuild)
        {
            var parent = GameObject.Find("InfoCanvasParent");
            if (parent == null)
                parent = new GameObject("InfoCanvasParent");

            infoCanvas.transform.SetParent(parent.transform);
        }

        foreach (Transform item in transform)
        {
            if (item.tag == "ShieldBubble")
            {
                pShieldBubble = item.gameObject;
                pShieldBubble.SetActive(false);
                break;
            }
        }

    }
    public virtual void InitMinion(WalkNode n, Vector3 pTransform = default(Vector3))
    {
        hasBeenFreed = true;
        if (pTransform == default(Vector3))
            transform.position = n.transform.position;
        else
            transform.position = pTransform;

        pNextNode = n.GetNextWalkNode();

    }
    public void SetWalk(bool val)
    {
        pCanWalk = val;
    }

    protected virtual void Start ()
    {
        Init();
	}
    protected virtual void Update () {
        PerformAction();
	}
    #endregion

    protected void FinishWalk()
    {
        pCanWalk = false;
        if (infoCanvas != null)
            Destroy(infoCanvas.gameObject);
        OnWalkFinished(this);
    }
    void DeathChecker()
    {
        if (IsDead)
        {
            if(infoCanvas != null)
                Destroy(infoCanvas.gameObject);
            GetComponent<Collider>().enabled = false;
            pCanWalk = false;
            if (_hasSparkEffect)
            {
                var ps = GetComponentInChildren<SimpleParticleSystem>();
                if(ps != null)
                    ps.Stop();
            }

            /*if (explotion != null)
            {
                explotion.Play(true);
                explotion.GetComponentInChildren<Animation>().Play("test");
                StartCoroutine(ExplotionStopped(explotion.main.duration));
            }*/
            OnDeath(this);
        }
    }

    IEnumerator ExplotionStopped(float t)
    {
        yield return new WaitForSeconds(t);
        OnDeath(this);
    }

    void CheckPSSpark()
    {
        if (sparkParticleSys == null) return;

        if (hp > (_initHP * 0.5f)) return;
        SimpleParticleSystem ps;
        if (!_hasSparkEffect)
        {
            ps = Instantiate(sparkParticleSys, transform);
            _hasSparkEffect = true;
        }
        else
            ps = GetComponentInChildren<SimpleParticleSystem>();

        if(ps != null)
            ps.IncrementBurst();
    }

    /// <summary>
    /// Main skill, the one at index 0. 
    /// return true if it is beeing regenerated or is active, false if both aren't.
    /// </summary>
    public bool IsMainSkillLockedOrActive()
    {
        if (pMainSkill == null)
            return false;

        return pMainSkill.IsLocked || pMainSkill.IsActivated;
    }
}
