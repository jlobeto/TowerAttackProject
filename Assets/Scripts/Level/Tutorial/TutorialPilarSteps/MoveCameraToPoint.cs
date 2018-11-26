using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraToPoint : StepBase
{
    TutorialPilarManager tutoPilarManager;

    public MoveCameraToPoint(TutorialPilarManager tutoOne)
    {
        tutoPilarManager = tutoOne;
    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        tutoPilarManager.lvl.isTutorial = true;
        Time.timeScale = 0;

        var pos = Camera.main.WorldToScreenPoint(tutoPilarManager.pilars[0].transform.position);
        Camera.main.transform.parent.GetComponent<CameraMovement>()
            .StartCameraMoveForTutorial(pos, 2f, tutoPilarManager.lvl.ExecuteTutorialStep);

    }
}
