using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerHasBeenKilled : StepBase {

	public RunnerHasBeenKilled(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.forRunner.OnTriggerEnterCallback += lvlTuto.OnRunnerColEnter;
		lvlTuto.GameManager.popupManager.BuildPopup(lvlTuto.LevelCanvasManager.transform
			, "Ups"
			, "Your minion has been killed. Try using his skill!"
			, "Try Again");

	}
}
