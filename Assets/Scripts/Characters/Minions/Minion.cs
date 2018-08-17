using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public int hp = 50;
    public CharacterType type = CharacterType.None;
    public MinionType minionType = MinionType.Runner;
    public float speed = 4;
    public int pointsValue = 15;
    public float strength = 0;
    public IMinionSkill skill;
    public int coinValue = 0;
    [Range(0f,1f)]
    public float levelPointsToRecover = 0.75f;

    int _currentLevel = 1;//Level of the minion, ///TODO manage this when buying an upgrade of lvl;
    WalkNode _nextNode;
    int _spawnOrder;
    int _id;
    bool _canWalk;
    float _distanceToNextNode = 0.3f;//To change the next node;

    public int Id { get { return _id; } }
    public bool CanWalk { get { return _canWalk; } }

    public void InitMinion(WalkNode n)
    {
        _nextNode = n.GetNextWalkNode();
    }

    public void SetWalk(bool val)
    {
        _canWalk = val;
    }

    protected virtual void PerformAction()
    {
        if (_canWalk)
            Walk();
    }

    protected virtual void Walk()
    {
        var dir = (_nextNode.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, _nextNode.transform.position) <= _distanceToNextNode)
            _nextNode = _nextNode.GetNextWalkNode();
    }

    protected void Init()
    {
        _id = gameObject.GetInstanceID();
    }

    void Start ()
    {
        Init();
	}
	
	
	void Update () {
        PerformAction();
	}

    
}
