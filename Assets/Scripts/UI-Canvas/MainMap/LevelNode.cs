using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour 
{
    public Sprite unlocked;
    public Sprite locked;

	LevelInfo _lvlInfo;
	GameManager _gm;
    Text _txt;
    Button _btn;

	void Awake () 
	{
        _txt = GetComponentInChildren<Text>();
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
        {
            _txt.text = "" + _lvlInfo.id;
            return;
        }
            

        var lastLevelProgress = _gm.User.LevelProgressManager.GetProgress(_lvlInfo.id -1);
        if(lastLevelProgress != null)
        {
            if (!lastLevelProgress.Won)
                _btn.interactable = false;
        }
        else
            _btn.interactable = false;

        if(!_btn.interactable)//change to lock.
        {
            GetComponent<Image>().sprite = locked;
        }

        _txt.text = _btn.interactable ? "" + _lvlInfo.id : "";
    }
}
