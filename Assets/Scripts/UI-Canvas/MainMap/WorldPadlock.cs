using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPadlock : MonoBehaviour
{
    Image _padlock;
    Image _blackScreen;
    Image _star;
    Text _worldLockedText;
    Text _neededToUnlockText;
    Text _needText;
    Text _mustWinLvls;

    void Awake ()
    {
        SetData();
    }

    public void SetLockUI(bool unlock, int toUnlock )
    {
        //if this is null, all of the variables are.
        //this happend when forcing al leves and the awake does not execute (this instance already exists
        if (_padlock == null)
            SetData();

        if (toUnlock != 0)
        {
            _neededToUnlockText.text = toUnlock.ToString();
            _neededToUnlockText.enabled = true;
            _worldLockedText.enabled = _padlock.enabled = _blackScreen.enabled = true;
            _needText.enabled = true;
            _star.enabled = true;
			_mustWinLvls.enabled = false;

        }
		else if (!unlock)
        {
            _worldLockedText.enabled = _padlock.enabled = _blackScreen.enabled = true;
            _mustWinLvls.enabled = true;
            _needText.enabled = false;
            _star.enabled = false;
        }
		else
		{
			_worldLockedText.enabled = _padlock.enabled = _blackScreen.enabled = false;
			_mustWinLvls.enabled = false;
			_needText.enabled = false;
			_star.enabled = false;
		}
        
    }

    void SetData()
    {
        var imgs = GetComponentsInChildren<Image>();
        foreach (var item in imgs)
        {
            if (item.name == "padlock")
                _padlock = item;
            else if (item.name == "blackScreen")
                _blackScreen = item;
            else if (item.name == "star")
                _star = item;
        }

        var txts = GetComponentsInChildren<Text>();
        foreach (var item in txts)
        {
            if (item.name == "worldLockedText")
                _worldLockedText = item;
            else if (item.name == "quantity")
                _neededToUnlockText = item;
            else if (item.name == "needText")
                _needText = item;
            else if (item.name == "mustWinLevels")
                _mustWinLvls = item;
        }
    }
	
}
