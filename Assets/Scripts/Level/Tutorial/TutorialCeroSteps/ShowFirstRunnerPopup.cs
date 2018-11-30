using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFirstRunnerPopup : StepBase 
{
	public ShowFirstRunnerPopup(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.GameManager.popupManager.BuildOneButtonPopup(lvlTuto.LevelCanvasManager.transform
			, "Well done!"
			, "You arrived to the end of path."
			, "Continue"
			, PopupsID.TutorialCero);
	}
}
