using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPilarTutotial : StepBase
{
    TutorialPilarManager tutoPilarManager;
    CameraMovement _camMovement;

    public StopPilarTutotial(TutorialPilarManager tutoOne)
    {
        tutoPilarManager = tutoOne;
        _camMovement = GameObject.FindObjectOfType<CameraMovement>();
    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        tutoPilarManager.lvl.isTutorial = false;
        Time.timeScale = 1;

        tutoPilarManager.canvasTuto.DisableTexts();
        tutoPilarManager.canvasTuto.DisableAllArrows();

        tutoPilarManager.CanClick = false;

        _camMovement.SetCameraMovement(true);
    }
}
