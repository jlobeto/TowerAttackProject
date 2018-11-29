using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour 
{
	LevelInfo _lvlInfo;
	GameManager _gm;

    Button _btn;

	void Start () 
	{
        
    }
	

	void Update () 
	{
		
	}

	public void Init(LevelInfo lvlInfo, GameManager gm, Button btn)
	{
		_lvlInfo = lvlInfo;
		_gm = gm;
        _btn = btn;
        RealInit ();
	}

    

	void RealInit()
	{
        if (_lvlInfo.id == 0)
            return;

        if(_gm.User == null)
        {
            Debug.Log(_lvlInfo.id + " = id ");
        }
        var lastLevelProgress = _gm.User.LevelProgressManager.GetProgress(_lvlInfo.id -1);
        if(lastLevelProgress != null)
        {
            if (!lastLevelProgress.Won)
                _btn.interactable = false;
        }
        else
            _btn.interactable = false;

    }
}
