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
    //public IMinionSkill skill;
    public int coinValue = 0;
    [Range(0f,1f)]
    public float levelPointsToRecover = 0.75f;
    public Action<Minion> OnWalkFinished = delegate { };
    public Action<Minion> OnDeath = delegate { };
    public InfoCanvas infoCanvas;
    public SimpleParticleSystem explotionPS;
    public List<BaseMinionSkill> skills = new List<BaseMinionSkill>();
    public float skillTime = 2;
    
    protected WalkNode pNextNode;
    protected float pDistanceToNextNode = 0.2f;//To change the next node;
    protected bool pIceDebuff;
    protected bool pBuffInvisible;//like runner run boost skill.

    float _initHP;
    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    int _spawnOrder;
    float _normalSpeed;
    float _iceTime;
    int _id;
    bool _canWalk;
    bool _hasExplotionEffect;

    public int Id { get { return _id; } }
    public bool CanWalk { get { return _canWalk; } }
    public bool IsTargetable { get { return !pBuffInvisible; } }


    public virtual void GetDamage(float dmg)
    {
        if (HasShieldBuff()) return;

        hp -= dmg;
        infoCanvas.UpdateLife(hp);
        CheckPSExplotion();
        DeathChecker();
    }

    public void GetSlowDebuff(float t, float speedDelta)
    {
        _iceTime = t;
        if (!pIceDebuff)
        {
            pIceDebuff = true;
            _normalSpeed = speed;
            speed -= speedDelta * speed;
        }
    }

    bool HasShieldBuff()
    {
        var shieldSkill = (ShieldSkill)BaseMinionSkill.GetSkillByType(BaseMinionSkill.SkillType.HitShield, skills);
        if (shieldSkill != null)
        {
            var enabled = shieldSkill.ExecuteSkill();
            if (this is Runner)
            {
                Debug.Log(enabled ? "shieldeed !! " : " you eat it D: ");
            }
            return enabled;
        }

        return false;
    }

    protected virtual void PerformAction()
    {
        infoCanvas.UpdatePosition(transform.position);

        if (_canWalk)
        {
            Walk();
        }

        if (pIceDebuff)
        {
            _iceTime -= Time.deltaTime;
            if (_iceTime < 0)
            {
                speed = _normalSpeed;
                pIceDebuff = false;
            }
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
        infoCanvas = Instantiate<InfoCanvas>(infoCanvas, transform.position, transform.rotation);
        infoCanvas.Init(hp);
        _initHP = hp;
        if (Debug.isDebugBuild)
        {
            var parent = GameObject.Find("InfoCanvasParent");
            if (parent == null)
                parent = new GameObject("InfoCanvasParent");

            infoCanvas.transform.SetParent(parent.transform);
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
        Destroy(infoCanvas.gameObject);
        OnWalkFinished(this);
    }
    void DeathChecker()
    {
        if (hp <= 0)
        {
            Destroy(infoCanvas.gameObject);
            OnDeath(this);
        }
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

        ps.IncrementBurst();
    }

    
}
