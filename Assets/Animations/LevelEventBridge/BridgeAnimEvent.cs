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

    public void OnStopAnimDown()
    {
        _anim.SetBool("startFall", false);
    }

    public void OnStopAnimUp()
    {
        _anim.SetBool("startUp", false);
    }
}
