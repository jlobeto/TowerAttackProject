using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerUseTheSkill : StepBase 
{
	public RunnerUseTheSkill(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.LevelCanvasManager.EnableMinionButtons(true);
		lvlTuto.LevelCanvasManager.StartTapAnimation (MinionType.Runner);
	}
}
