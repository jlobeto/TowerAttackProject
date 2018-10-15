using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase
{

    Minion _target;
    LineRenderer _lineRender;
    float _checkForMinionTime = 0.2f;
    float _timeShooting = 1;
    float _timeShootingAux = 1;
    float _targetHPToGo; //this is the minion hp that is going to have after 'timeShooting' time.

    protected override void Start()
    {
        base.Start();
        _lineRender = GetComponentInChildren<LineRenderer>();
        _lineRender.enabled = false;
        //_lineRender.SetPositions(new Vector3[] { spawnPoint.position, spawnPoint.position });
        
    }

    protected override void Update()
    {
        if (!isInitialized)
            return;

        if (!pImStunned)
        {
            CheckForMinionAppeareance();
            MinionAiming();
            Fire();
        }
    }

    protected override void Fire()
    {
        if (pTarget == null)
        {
            if (_lineRender.enabled)
                _lineRender.enabled = false;

            _timeShooting = _timeShootingAux;
            return;
        }

        if (!_lineRender.enabled)
        {
            _target = pTarget.GetComponent<Minion>();
            _lineRender.enabled = true;
            //_targetHPToGo = _target.hp - pMyStat.dmgPerSecond;
            _target.GetDamage(pMyStat.dmgPerSecond);
        }

        /*_target.hp = Mathf.Lerp(_target.hp, _targetHPToGo, Time.deltaTime);
        Debug.Log(_target.hp);*/

        _timeShooting -= Time.deltaTime;
        if (_timeShooting < 0)
        {
            _timeShooting = _timeShootingAux;
            _target.GetDamage(pMyStat.dmgPerSecond);
        }

        _lineRender.SetPosition(0, spawnPoint.position);
        if (_target != null)
            _lineRender.SetPosition(1, _target.transform.position);
        else
            pTarget = null;
    }

    protected override void OnTargetHasChanged()
    {
        //Debug.Log("OnTargetHasChanged()");
        _target = pTarget.GetComponent<Minion>();
    }

    /// <summary>
    /// Don't want to execute GetTarget() all frames.
    /// </summary>
    void CheckForMinionAppeareance()
    {
        _checkForMinionTime -= Time.deltaTime;
        if (_checkForMinionTime < 0)
        {
            _checkForMinionTime = 0.2f;
            GetTarget();
        }
    }

}
