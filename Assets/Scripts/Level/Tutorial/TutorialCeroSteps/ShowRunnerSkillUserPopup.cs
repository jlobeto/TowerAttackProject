using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRunnerSkillUserPopup : StepBase
{

	public ShowRunnerSkillUserPopup(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.GameManager.popupManager.BuildOneButtonPopup(lvlTuto.LevelCanvasManager.transform
			, "Awesome!"
			, "You know how to use the runner skill"
			, "Continue"
			, PopupsID.TutorialCero);
	}
}
