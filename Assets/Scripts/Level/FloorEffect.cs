using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorEffect : MonoBehaviour {

    List<BlockEffect> _blocks = new List<BlockEffect>();
    Animator _anim;
	void Start () {
        _blocks = GetComponentsInChildren<BlockEffect>().ToList();
    }
	
	
	void Update () {
		
	}

    public void InitAnimation()
    {
        foreach (var item in _blocks)
        {
            item.InitAnimation();
        }
    }
}
