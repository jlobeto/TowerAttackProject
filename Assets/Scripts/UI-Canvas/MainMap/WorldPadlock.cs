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

    void Awake ()
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
        }
	}

    public void SetLockUI(bool unlock, int toUnlock )
    {
        _worldLockedText.enabled = _padlock.enabled = _blackScreen.enabled = !unlock;

        _neededToUnlockText.text = toUnlock.ToString();
        _neededToUnlockText.enabled = !unlock;
        _needText.enabled = !unlock;
        _star.enabled = !unlock;
    }
	
}
