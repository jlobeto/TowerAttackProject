using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BridgeClock : MonoBehaviour
{
    public string bridgePivot;
    //public MeshRenderer clockRenderer;
    public Text panelText;
    public ParticleSystem sparks_1;
    public ParticleSystem sparks_2;

    bool _startCountDown;
    float _timeAux;
    float _initTime;
    float _emissionQty;
    float _emissionQtyStart;
    float _lerpCurrentTime;

    void Start () {
        //_emissionQtyStart = _emissionQty = clockRenderer.sharedMaterial.GetFloat("_EmissionQty");

    }

    public void StartCountdown(float time)
    {
        _startCountDown = true;
        _initTime = _timeAux = time;
        panelText.text = _initTime+"";
        _lerpCurrentTime = 0;
        StartCoroutine(PanelTextChange());
    }
	
    IEnumerator PanelTextChange()
    {
        yield return new WaitForSeconds(1);
        panelText.text = _timeAux.ToString("0");

        if(_startCountDown)
            StartCoroutine(PanelTextChange());
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
            }
            
        }
	}
}
