using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeAnimEvent : MonoBehaviour
{

    Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void OnStopAnim()
    {
        _anim.SetBool("startFall", false);
    }
}
