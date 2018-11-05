using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCeroOneBtnPopup : OneBtnPopup
{
    LevelCeroTutorial _level;

    protected override void Start()
    {
        _level = FindObjectOfType<LevelCeroTutorial>();
    }

    public override void OnActionButton()
    {
        _level.OnPopupButtonPressed();
        Destroy(gameObject);
    }
}
