using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParticleSystem : MonoBehaviour
{
    ParticleSystem _system;
    float _burstCount;
	void Start ()
    {
        _system = GetComponent<ParticleSystem>();
        _system.Play();
        _burstCount = _system.emission.GetBurst(0).count.constant;
    }

    public void IncrementBurst()
    {
        _burstCount *= 3;
        if(_system != null)
            _system.emission.SetBurst(0, new ParticleSystem.Burst(0, _burstCount));
    }

	// Update is called once per frame
	void Update () {
		
	}
}
