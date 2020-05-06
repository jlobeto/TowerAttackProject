using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase
{
    public ParticleSystem laserSpawnPS;
    public ParticleSystem laserHitPS;

    Minion _minionTargeted;
    LineRenderer _lineRender;
    Vector3 _directionToAimOnNewTarget;//this will be used only when a target changes or if it is the first target
    bool _effectsEnabled;
    float _checkForMinionTime = 0.2f;
    float _checkForMinionTimeAux;
    float _timeShooting = 1;
    float _timeShootingAux = 1;
    bool _hasToRotateTowardsNewTarget;
    float _timeRotatingTowardsNewTarget;

    protected override void Start()
    {
        base.Start();
        _lineRender = GetComponentInChildren<LineRenderer>();
        EnableEffects(false);
        _checkForMinionTimeAux = _checkForMinionTime;
    }

    public override void ReceiveStun(float time)
    {
        base.ReceiveStun(time);
        EnableEffects(false);
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

    protected override void MinionAiming()
    {
        if (pTarget == null) return;

        var dir = _hasToRotateTowardsNewTarget ? _directionToAimOnNewTarget : pTarget.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        var t = Time.deltaTime * pMyStat.rotationSpeed;
        Vector3 rotation = Quaternion.Lerp(toRotate.transform.rotation, lookRotation, t).eulerAngles;
        toRotate.transform.rotation = Quaternion.Euler(0f, rotation.y, 0);

        _timeRotatingTowardsNewTarget += t;

        if (_hasToRotateTowardsNewTarget && _timeRotatingTowardsNewTarget > 0.9f)
        {
            _hasToRotateTowardsNewTarget = false;
            _timeRotatingTowardsNewTarget = 0;
        }
    }

    protected override void Fire()
    {
        if (pTarget == null)
        {
            EnableEffects(false);
            _timeShooting = _timeShootingAux;
            return;
        }

        if (!_effectsEnabled && !_hasToRotateTowardsNewTarget)
        {
            EnableEffects(true);
            _minionTargeted.GetDamage(pMyStat.dmgPerSecond);
        }
        
        if(!_hasToRotateTowardsNewTarget)
        {
            _timeShooting -= Time.deltaTime;
            if (_timeShooting < 0)
            {
                _timeShooting = _timeShootingAux;
                _minionTargeted.GetDamage(pMyStat.dmgPerSecond);
            }

            CalculateEffectsPositions();
        }
    }

    void CalculateEffectsPositions()
    {
        Vector3 dir;
        dir = spawnPoint.position - pTarget.transform.position;
        laserSpawnPS.transform.position = spawnPoint.position;

        _lineRender.SetPosition(0, spawnPoint.position);
        _lineRender.SetPosition(1, pTarget.transform.position);

        laserHitPS.transform.position = pTarget.transform.position + dir.normalized;
        laserHitPS.transform.rotation = Quaternion.LookRotation(dir);
    }

    /// <summary>
    /// Don't want to execute GetTarget() all frames.
    /// </summary>
    void CheckForMinionAppeareance()
    {
        _checkForMinionTimeAux -= Time.deltaTime;
        if (_checkForMinionTimeAux < 0)
        {
            _checkForMinionTimeAux = _checkForMinionTime;
            GetTarget();

            if(pTarget != null)
            {
                if(_minionTargeted == null || _minionTargeted.gameObject.GetInstanceID() != pTarget.GetInstanceID())
                {
                    _minionTargeted = pTarget.GetComponent<Minion>();
                    _hasToRotateTowardsNewTarget = true;
                    _directionToAimOnNewTarget = pTarget.transform.position - transform.position;
                    EnableEffects(false);
                    _timeRotatingTowardsNewTarget = 0;
                }
            }
                
        }
    }

    void EnableEffects(bool enable)
    {
        _effectsEnabled = enable;
        if(enable)
        {
            _lineRender.enabled = true;
            laserSpawnPS.Play();
            laserHitPS.Play();
        }
        else
        {
            laserHitPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            laserSpawnPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _lineRender.enabled = false;
        }
    }

}
