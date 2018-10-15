using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerBase
{
    public Transform upSpawn;
    public Transform secondGrounSpawn;
    public ParticleSystem laserSpawnPS;

    Minion _target;
    LineRenderer _lineRender;
    float _checkForMinionTime = 0.2f;
    float _timeShooting = 1;
    float _timeShootingAux = 1;
    float _targetHPToGo; //this is the minion hp that is going to have after 'timeShooting' time.
    bool _leftSpawn;

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
            {
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

        if (_target != null)
        {
            if (_target.targetType == TargetType.Ground)
            {
                if (_leftSpawn)
                {
                    laserSpawnPS.transform.position = spawnPoint.position;
                    _lineRender.SetPosition(0, spawnPoint.position);
                }
                else
                {
                    laserSpawnPS.transform.position = secondGrounSpawn.position;
                    _lineRender.SetPosition(0, secondGrounSpawn.position);
                }
            }
            else
            {
                laserSpawnPS.transform.position = upSpawn.position;
                _lineRender.SetPosition(0, upSpawn.position);
            }



            _lineRender.SetPosition(1, _target.transform.position);
        }
        else
        {
            _leftSpawn = !_leftSpawn;
            pTarget = null;
        }
            
    }

    protected override void OnTargetHasChanged()
    {
        //Debug.Log("OnTargetHasChanged()");
        _leftSpawn = !_leftSpawn;
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
