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

    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    WalkNode _nextNode;
    int _spawnOrder;
    int _id;
    bool _canWalk;
    float _distanceToNextNode = 0.3f;//To change the next node;
    InfoCanvas _infoCanvas;

    public int Id { get { return _id; } }
    public bool CanWalk { get { return _canWalk; } }

    public void InitMinion(WalkNode n)
    {
        _nextNode = n.GetNextWalkNode();
    }

    public void SetWalk(bool val)
    {
        if (_nextNode.isEnd) return;//don't know if this will be here, for testing porpuse must be for the moment.

        _canWalk = val;
    }

    public void GetDamage(float dmg)
    {
        hp -= dmg;
        _infoCanvas.UpdateLife(hp);
        DeathChecker();
    }

    protected virtual void PerformAction()
    {
        if (_canWalk)
            Walk();
    }

    protected virtual void Walk()
    {
        var dir = (_nextNode.transform.position - transform.position).normalized;
        transform.forward = dir;
        transform.position += transform.forward * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, _nextNode.transform.position) <= _distanceToNextNode)
        {
            if (!_nextNode.isEnd)
                _nextNode = _nextNode.GetNextWalkNode();
            else
                FinishWalk();
        }
    }

    protected void Init()
    {
        _id = gameObject.GetInstanceID();
        _infoCanvas = GetComponentInChildren<InfoCanvas>();
        if (_infoCanvas == null)
            throw new Exception("InfoCanvas is not set as a child");

        _infoCanvas.Init(hp);
    }

    protected virtual void Start ()
    {
        Init();
	}


    protected virtual void Update () {
        PerformAction();
	}

    void DeathChecker()
    {
        if (hp <= 0)
            OnDeath(this);
    }

    void FinishWalk()
    {
        _canWalk = false;
        OnWalkFinished(this);
    }
}
