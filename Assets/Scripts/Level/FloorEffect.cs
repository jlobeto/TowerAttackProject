using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEffect : MonoBehaviour {


    Animator _anim;
	void Start () {
        _anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitAnimation()
    {
        if(_anim.GetBool("FloorBGAnim"))
            _anim.SetBool("FloorBGAnim", false);

        _anim.SetBool("FloorBGAnim", true);
    }

    public void OnFinishFloorBGAnim()
    {
        _anim.SetBool("FloorBGAnim", false);
    }
}
