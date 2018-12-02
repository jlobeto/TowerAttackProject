using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFirstText : StepBase
{
    TutorialElectroshockManager _tutoMan;
    string _textName;
    CameraMovement _camMovement;

    public ShowFirstText(string textName, TutorialElectroshockManager t)
    {
        _camMovement = GameObject.FindObjectOfType<CameraMovement>();
        _textName = textName;
        _tutoMan = t;
    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        _camMovement.SetCameraMovement(false);
        _tutoMan.CanClick = true;
        _tutoMan.lvl.isTutorial = true;
        Time.timeScale = 0;

        _tutoMan.canvasTuto.EnableArrowByName("arrow_1");
        var pos = Camera.main.WorldToScreenPoint(_tutoMan.electroshockManager.position);
        _tutoMan.canvasTuto.SetArrowParentPosition(pos, "arrow_1", 20);

        _tutoMan.canvasTuto.DisableTexts();

        _tutoMan.canvasTuto.EnableDisableTextByName(_textName, true);

    }
}
