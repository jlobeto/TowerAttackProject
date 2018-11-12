using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRunnerEnterToUseSkill : StepBase {


	public OnRunnerEnterToUseSkill(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.LevelCanvasTutorial.EnableArrowByName("PointingToRunnerSkill");
		var pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		lvlTuto.LevelCanvasTutorial.SetArrowPosition(pos, "PointingToRunnerSkill", 40);
		Time.timeScale = 0;
	}
}
