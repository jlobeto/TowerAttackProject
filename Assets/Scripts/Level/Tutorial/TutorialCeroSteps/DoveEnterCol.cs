using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveEnterCol : StepBase {

	public DoveEnterCol(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		Time.timeScale = 0;

		var antiAir = lvlTuto.TowerManager.GetTowerByTypeAndName (TowerType.Antiair, "tuto2");
		var cannon = lvlTuto.TowerManager.GetTowerByTypeAndName (TowerType.Cannon, "tuto2");

		lvlTuto.LevelCanvasTutorial.EnableArrowByName("SecondPointer");
		var pos = Camera.main.WorldToScreenPoint(antiAir.transform.position);
		lvlTuto.LevelCanvasTutorial.SetArrowPosition(pos, "SecondPointer", 40);

		lvlTuto.LevelCanvasTutorial.EnableArrowByName("ThirdPointer");
		pos = Camera.main.WorldToScreenPoint(cannon.transform.position);
		lvlTuto.LevelCanvasTutorial.SetArrowPosition(pos, "ThirdPointer", 40);

	}
}
