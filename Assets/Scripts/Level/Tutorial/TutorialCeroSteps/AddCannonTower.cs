using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCannonTower : StepBase
{
	public AddCannonTower(LevelCeroTutorial tuto = null)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.TowerManager.ShowTowerByTypeAndName(TowerType.Cannon, "tuto1");
		lvlTuto.LevelCanvasManager.EnableMinionButtons(true);
		lvlTuto.LevelCanvasManager.StartTapAnimation (MinionType.Runner);
	}
}
