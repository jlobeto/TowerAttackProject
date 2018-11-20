using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTapOnRunnerInTankTuto : StepBase
{
	bool _run;
	public AddTapOnRunnerInTankTuto(LevelCeroTutorial tuto, bool runner)
	{
		lvlTuto = tuto;
		_run = runner;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		Time.timeScale = 0;

		if (!_run)
		{
			lvlTuto.LevelCanvasManager.EnableMinionSaleSpecific (true, MinionType.Dove);
			lvlTuto.LevelCanvasManager.StartTapAnimation (MinionType.Dove,true);
		}
		else 
		{
			lvlTuto.LevelCanvasManager.EnableMinionSaleSpecific (true, MinionType.Runner);

			lvlTuto.LevelCanvasManager.StartTapAnimation (MinionType.Runner,true);
		}
			
	}
}
