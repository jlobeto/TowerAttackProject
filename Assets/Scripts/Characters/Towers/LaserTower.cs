using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase
{
    public ParticleSystem laserSpawnPS;
    public ParticleSystem laserHitPS;

    Minion _target;
    LineRenderer _lineRender;
    float _checkForMinionTime = 0.2f;
    float _timeShooting = 1;
    float _timeShootingAux = 1;

    protected override void Start()
    {
        base.Start();
        _lineRender = GetComponentInChildren<LineRenderer>();
        _lineRender.enabled = false;
        //_lineRender.SetPositions(new Vector3[] { spawnPoint.position, spawnPoint.position });
        
    }

    public override void ReceiveStun(float time)
    {
        base.ReceiveStun(time);

        laserHitPS.Stop();
        laserSpawnPS.Stop();
        _lineRender.enabled = false;
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
            {
                laserHitPS.Stop();
                laserSpawnPS.Stop();
                _lineRender.enabled = false;
            }
            _timeShooting = _timeShootingAux;
            
            return;
        }

        if (!_lineRender.enabled)
        {
            _target = pTarget.GetComponent<Minion>();
            _lineRender.enabled = true;
            laserSpawnPS.Play();
            _target.GetDamage(pMyStat.dmgPerSecond);
            laserHitPS.Play();
        }
        
        _timeShooting -= Time.deltaTime;
        if (_timeShooting < 0)
        {
            _timeShooting = _timeShootingAux;
            _target.GetDamage(pMyStat.dmgPerSecond);
        }

        if (_target != null)
        {
            Vector3 dir;

			laserSpawnPS.transform.position = spawnPoint.position;
			_lineRender.SetPosition(0, spawnPoint.position);
			dir = spawnPoint.position - _target.transform.position;

            _lineRender.SetPosition(1, _target.transform.position);
            laserHitPS.transform.position = _target.transform.position + dir.normalized;
            laserHitPS.transform.rotation = Quaternion.LookRotation(dir);

        }
        else
        {
            laserHitPS.transform.position = spawnPoint.transform.position;
            laserHitPS.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
        }
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
