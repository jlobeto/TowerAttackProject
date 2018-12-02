using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopElectroshockTuto : StepBase
{
    TutorialElectroshockManager _tutoMan;
    CameraMovement _camMovement;


    public StopElectroshockTuto( TutorialElectroshockManager t)
    {
        _tutoMan = t;
        _camMovement = GameObject.FindObjectOfType<CameraMovement>();

    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        _tutoMan.lvl.isTutorial = false;
        Time.timeScale = 1;

        _tutoMan.canvasTuto.DisableTexts();
        _tutoMan.canvasTuto.DisableAllArrows();

        _tutoMan.CanClick = false;

        _camMovement.SetCameraMovement(true);

    }
}