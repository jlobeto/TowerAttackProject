using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour 
{
    public Sprite unlocked;
    public Sprite locked;
    public Sprite starOnSprite;

	LevelInfo _lvlInfo;
	GameManager _gm;
    Text _txt;
    Button _btn;
    List<Image> _stars;


	void Awake () 
	{
        _txt = GetComponentInChildren<Text>();
        _stars = GetComponentsInChildren<Image>().Skip(1).ToList();
    }
	

	void Update () 
	{
		
	}

    public void Init(LevelInfo lvlInfo, GameManager gm, Button btn)
    {
        if (lvlInfo.id == 0)
            foreach (var item in _stars)
                item.enabled = false;

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

        var currentLevelProgress = _gm.User.LevelProgressManager.GetProgress(_lvlInfo.id);
        if(currentLevelProgress != null)
            for (int i = 0; i < currentLevelProgress.StarsWon; i++)
                _stars[i].sprite = starOnSprite;

        if (!_btn.interactable)//change to lock.
        {
            GetComponent<Image>().sprite = locked;
        }

        _txt.text = _btn.interactable ? "" + _lvlInfo.id : "";
    }
}
