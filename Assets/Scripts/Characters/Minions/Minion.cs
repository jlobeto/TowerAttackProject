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
    public float speed = 4;
    public int pointsValue = 15;
    public float strength = 0;
    public IMinionSkill skill;
    public int coinValue = 0;
    [Range(0f,1f)]
    public float levelPointsToRecover = 0.75f;
    public Action<Minion> OnWalkFinished = delegate { };
    public Action<Minion> OnDeath = delegate { };
    public InfoCanvas infoCanvas;

    protected WalkNode pNextNode;
    
    protected float pDistanceToNextNode = 0.2f;//To change the next node;

    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    int _spawnOrder;
    float _normalSpeed;
    bool _iceDebuff;
    float _iceTime;
    int _id;
    bool _canWalk;

    public int Id { get { return _id; } }
    public bool CanWalk { get { return _canWalk; } }

    public void InitMinion(WalkNode n)
    {
        transform.position = n.transform.position;
        pNextNode = n.GetNextWalkNode();
    }
    public void SetWalk(bool val)
    {
        if (pNextNode.isEnd) return;//don't know if this will be here, for testing porpuse must be for the moment.

        _canWalk = val;
    }
    public void GetDamage(float dmg)
    {
        hp -= dmg;
        infoCanvas.UpdateLife(hp);
        DeathChecker();
    }

    public void GetSlowDebuff(float t, float speedDelta)
    {
        _iceTime = t;
        if (!_iceDebuff)
        {
            _iceDebuff = true;
            _normalSpeed = speed;
            speed -= speedDelta * speed;
        }
    }
    
    protected virtual void PerformAction()
    {
        infoCanvas.UpdatePosition(transform.position);

        if (_canWalk)
            Walk();

        if (_iceDebuff)
        {
            _iceTime -= Time.deltaTime;
            if (_iceTime < 0)
            {
                speed = _normalSpeed;
                _iceDebuff = false;
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
    protected void Init()
    {
        _id = gameObject.GetInstanceID();
        infoCanvas = Instantiate<InfoCanvas>(infoCanvas, transform.position, transform.rotation);
        infoCanvas.Init(hp);
        if (Debug.isDebugBuild)
        {
            var parent = GameObject.Find("InfoCanvasParent");
            if (parent == null)
                parent = new GameObject("InfoCanvasParent");

            infoCanvas.transform.SetParent(parent.transform);
        }
    }
    protected virtual void Start ()
    {
        Init();
	}
    protected virtual void Update () {
        PerformAction();
	}
    protected void FinishWalk()
    {
        _canWalk = false;
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

    
}
