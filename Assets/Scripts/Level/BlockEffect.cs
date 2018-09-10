using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is used for level floor, each separate block is a BlockEffect;
/// </summary>
public class BlockEffect : MonoBehaviour
{

    Animator _anim;
	void Start () {
        _anim = GetComponent<Animator>();
    }

    public void InitAnimation()
    {
        if (_anim.GetBool("FloorBGAnim"))
            _anim.SetBool("FloorBGAnim", false);

        _anim.SetBool("FloorBGAnim", true);
    }

    public void OnFinishFloorBGAnim()
    {
        _anim.SetBool("FloorBGAnim", false);
    }

    void Update () {
		
	}
}
