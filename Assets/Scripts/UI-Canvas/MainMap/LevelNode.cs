using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNode : MonoBehaviour 
{
    public Sprite locked;
    public Sprite starOnSprite;
    public Text txt;
    public List<Image> stars;

    LevelInfo _lvlInfo;
    GameManager _gm;
    Button _btn;
    


	void Awake () 
	{
    }
	

	void Update () 
	{
		
	}

    public void Init(LevelInfo lvlInfo, GameManager gm, Button btn)
    {
        if (lvlInfo.id == 0)
            foreach (var item in stars)
                item.enabled = false;

		_lvlInfo = lvlInfo;
		_gm = gm;
        _btn = btn;
        RealInit ();
	}

    

	void RealInit()
	{

        var lastLevelProgress = _gm.User.LevelProgressManager.GetProgress(_lvlInfo.id -1);
        if(lastLevelProgress != null)
        {
            if (!lastLevelProgress.won)
                _btn.interactable = false;
                
        }
        else if (_lvlInfo.id > 1 && _lvlInfo.mode != LevelMode.Tutorial.ToString())
            _btn.interactable = false;

        var currentLevelProgress = _gm.User.LevelProgressManager.GetProgress(_lvlInfo.id);
        if(currentLevelProgress != null)
            for (int i = 0; i < currentLevelProgress.starsWon; i++)
                stars[i].sprite = starOnSprite;

        if (!_btn.interactable)//change to lock.
        {
            GetComponent<Image>().sprite = locked;
        }

        txt.text = _btn.interactable ? "" + _lvlInfo.id : "";
    }
}
