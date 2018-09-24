using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public string minionName;
    public float hp = 50;
    public GameObjectTypes type = GameObjectTypes.None;
    public MinionType minionType = MinionType.Runner;
    public TargetType targetType = TargetType.Both;
    public float speed = 4;
    public int pointsValue = 15;
    public float strength = 0;
    public float spawnCooldown = 0.5f;
    //public IMinionSkill skill;
    public int coinValue = 0;
    [Range(0f,1f)]
    public float levelPointsToRecover = 0.75f;
    public Action<Minion> OnWalkFinished = delegate { };
    public Action<Minion> OnDeath = delegate { };
    public InfoCanvas infoCanvas;
    public SimpleParticleSystem explotionPS;
    [HideInInspector]
    public List<BaseMinionSkill> skills = new List<BaseMinionSkill>();
    public float skillTime = 2;
    public float skillCooldown = 5;

    protected bool pDamageDebuff;
    protected float pDamageDebuffValue;
    protected WalkNode pNextNode;
    protected GameObject pShieldBubble;
    protected Animator pAnimator;
    protected float pDistanceToNextNode = 0.2f;//To change the next node;
    protected bool pIceDebuff;
    protected bool pBuffInvisible;//like runner run boost skill.

    float _initHP;
    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    int _spawnOrder;
    float _normalSpeed;
    int _id;
    bool _canWalk;
    bool _hasExplotionEffect;

    public int Id { get { return _id; } }
    public bool CanWalk { get { return _canWalk; } }
    public bool IsTargetable { get { return !pBuffInvisible; } }
    public GameObject ShieldBubble { get { return pShieldBubble; } }

    #region Damage to Minion
    public virtual void GetDamage(float dmg)
    {
        if (hp <= 0) return;

        if (HasShieldBuff()) return;

        if (pDamageDebuff)
            dmg *= pDamageDebuffValue;

        hp -= dmg;
        infoCanvas.UpdateLife(hp);
        CheckPSExplotion();
        DeathChecker();
    }

    /// <summary>
    /// if t == 0, this minion won't manage the timer to remove debuff by it self,
    /// it will wait until another thing gives the call to remove the debuff.
    /// If t != 0, it will use a coroutine to remove debuff.(need to test that because is new)
    /// </summary>
    public void GetSlowDebuff(float t, float speedDelta)
    {
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
        _canWalk = false;
        StartCoroutine(FreezeDebuffTimer(t));
    }

    IEnumerator FreezeDebuffTimer(float t)
    {
        yield return new WaitForSeconds(t);
        _canWalk = true;
    }

    public void DamageDebuff(bool enabled, float dmgDelta = 1)
    {
        pDamageDebuff = enabled;
        pDamageDebuffValue = dmgDelta;
    }
    
    #endregion

    bool HasShieldBuff()
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

        if (_canWalk)
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
        throw new NotImplementedException("ActivateSkill() is not implemented on dependences.");
    }

    #region Inits, Start and Update
    protected void Init()
    {
        _id = gameObject.GetInstanceID();
        pAnimator = GetComponentInChildren<Animator>();

        infoCanvas = Instantiate<InfoCanvas>(infoCanvas, transform.position, transform.rotation);
        infoCanvas.Init(hp, skillTime, skillCooldown);
        _initHP = hp;
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
    public virtual void InitMinion(WalkNode n)
    {
        transform.position = n.transform.position;
        pNextNode = n.GetNextWalkNode();
    }
    public void SetWalk(bool val)
    {
        if (pNextNode.isEnd) return;//don't know if this will be here, for testing porpuse must be for the moment.

        _canWalk = val;
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
        _canWalk = false;
        if (infoCanvas != null)
            Destroy(infoCanvas.gameObject);
        OnWalkFinished(this);
    }
    void DeathChecker()
    {
        if (hp <= 0)
        {
            pAnimator.SetBool("RunDissolve", true);
            if(infoCanvas != null)
                Destroy(infoCanvas.gameObject);
            _canWalk = false;
            if (_hasExplotionEffect)
            {
                var ps = GetComponentInChildren<SimpleParticleSystem>();
                if(ps != null)
                    ps.Stop();
            }
        }
    }

    /// <summary>
    /// Animation Event
    /// </summary>
    public void DissolveStopped()
    {
        OnDeath(this);
    }

    void CheckPSExplotion()
    {
        if (hp > (_initHP * 0.5f)) return;
        SimpleParticleSystem ps;
        if (!_hasExplotionEffect)
        {
            ps = Instantiate(explotionPS, transform);
            _hasExplotionEffect = true;
        }
        else
            ps = GetComponentInChildren<SimpleParticleSystem>();

        if(ps != null)
            ps.IncrementBurst();
    }

    
}
