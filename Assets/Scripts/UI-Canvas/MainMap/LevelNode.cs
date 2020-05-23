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
        {
            for (int i = 0; i < currentLevelProgress.starsWon; i++)
                stars[i].sprite = starOnSprite;

            //if has progress, block the button so it can never be touched again
            if (_lvlInfo.mode == LevelMode.Tutorial.ToString())
            {
                if(_lvlInfo.id == -1)//but if is level '-1', check if phase has been completed
                {
                    if (_gm.tutorialManager.HasUserCompletedTutorial(TutorialPhase.FirstTimeOnApp_INGAME_tuto_1_phase3.ToString()))
                        _btn.interactable = false;
                }
                else
                    _btn.interactable = false;
            }
                
        }

        if (!_btn.interactable)//change to lock.
        {
            GetComponent<Image>().sprite = locked;
        }

        txt.text = _btn.interactable ? "" + _lvlInfo.id : "";
    }
}
