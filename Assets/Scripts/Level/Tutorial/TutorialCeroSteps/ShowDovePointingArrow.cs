using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowDovePointingArrow : StepBase 
{
	public ShowDovePointingArrow(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		Time.timeScale = 0;
		var m = lvlTuto.MinionManager.GetMinion (MinionType.Dove);

		lvlTuto.LevelCanvasTutorial.EnableArrowByName("PointingToRunnerSkill");
		var pos = Camera.main.WorldToScreenPoint(m.transform.position);
		lvlTuto.LevelCanvasTutorial.SetArrowPosition(pos, "PointingToRunnerSkill");
	}
}
