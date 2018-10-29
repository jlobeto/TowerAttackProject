using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeClock : MonoBehaviour
{
    public string bridgePivot;
    public MeshRenderer clockRenderer;
    public ParticleSystem sparks_1;
    public ParticleSystem sparks_2;

    bool _startCountDown;
    float _timeAux;
    float _initTime;
    float _emissionQty;
    float _emissionQtyStart;
    float _lerpCurrentTime;

    void Start () {
        _emissionQtyStart = _emissionQty = clockRenderer.material.GetFloat("_EmissionQty");
    }

    public void StartCountdown(float time)
    {
        _startCountDown = true;
        _initTime = _timeAux = time;
        _lerpCurrentTime = 0;
    }
	
	
	void Update ()
    {
        if (_startCountDown)
        {
            _timeAux -= Time.deltaTime;
            if (_timeAux < _initTime * 0.2f && !sparks_1.isPlaying)
            {
                sparks_1.Play(true);
            }
            else if (_timeAux < _initTime * 0.1f && !sparks_2.isPlaying)
            {
                sparks_2.Play(true);
            }
            else if (_timeAux < 0)
            {
                sparks_1.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                sparks_2.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                _startCountDown = false;
                _emissionQty = _emissionQtyStart;
            }

            if (_lerpCurrentTime <= _initTime)
            {
                
                _emissionQty = Mathf.Lerp(_emissionQtyStart, 20, _lerpCurrentTime );//puse -18 xq a partir de ahi ya se empieza a ver muy brillante
                clockRenderer.material.SetFloat("_EmissionQty",_emissionQty);
                _lerpCurrentTime += Time.deltaTime / _initTime;
                //Debug.Log(_emissionQty);
            }
            
        }
	}
}
