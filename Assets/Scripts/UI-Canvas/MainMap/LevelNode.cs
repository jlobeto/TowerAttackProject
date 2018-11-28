using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode : MonoBehaviour 
{
	LevelInfo _lvlInfo;
	GameManager _gm;

	void Start () 
	{
		
	}
	

	void Update () 
	{
		
	}

	public void Init(LevelInfo lvlInfo, GameManager gm)
	{
		_lvlInfo = lvlInfo;
		_gm = gm;
		RealInit ();
	}

	void RealInit()
	{
		var lvlProgress = _gm.User.LevelProgressManager.GetProgress (_lvlInfo.id);

		//if(!lvlProgress.Won && )
	}
}
