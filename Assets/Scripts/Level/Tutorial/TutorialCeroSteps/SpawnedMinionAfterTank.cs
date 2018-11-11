using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedMinionAfterTank : StepBase
{
	public SpawnedMinionAfterTank(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		if(gameObject.GetComponent<Minion>().minionType == MinionType.Dove)
			lvlTuto.LevelCanvasTutorial.DisableArrowByName("PressThirdBtn");
		else
			lvlTuto.LevelCanvasTutorial.DisableArrowByName("PressSecondBtn");
	}
}