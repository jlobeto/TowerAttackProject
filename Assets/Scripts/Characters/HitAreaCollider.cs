using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAreaCollider : MonoBehaviour
{
    public Action<Collider> OnTriggerEnterCallback = delegate { };
    public Action<Collider> OnTriggerExitCallback = delegate { };
    public Action<Collider> OnTriggerStayCallback = delegate { };

    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterCallback(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitCallback(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayCallback(other);
    }
}
