using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleSystem : MonoBehaviour
{
    ParticleSystem _system;
    ParticleSystem.MainModule _main;
    float _burstCount;
    bool _stop;
	void Start ()
    {
        _system = GetComponent<ParticleSystem>();
        _main = _system.main;
        _system.Play();
        _burstCount = _system.emission.GetBurst(0).count.constant;
    }

    public void IncrementBurst()
    {
        _burstCount *= 3;
        if(_system != null)
            _system.emission.SetBurst(0, new ParticleSystem.Burst(0, _burstCount));
    }

    public void Stop()
    {
        _stop = true;
    }

	void Update ()
    {
        if (!_stop) return;


        var color = _main.startColor.color;

        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 2f);
        _main.startColor = color;
        if (color.a < 0.1f)
            Destroy(gameObject);
    }
}
