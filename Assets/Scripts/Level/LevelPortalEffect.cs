using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortalEffect : MonoBehaviour
{

    public ParticleSystem portalPS;

	void Start ()
    {
	    	
	}
	
	
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Minion"))
        {
            portalPS.Emit(15);
        }
    }
}
