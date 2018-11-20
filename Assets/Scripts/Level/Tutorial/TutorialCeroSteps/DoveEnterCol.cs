using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoveEnterCol : StepBase {

	public DoveEnterCol(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		//Time.timeScale = 0;

		var m = lvlTuto.MinionManager.GetMinion (MinionType.Dove);
		m.SetWalk (false);

		var antiAir = lvlTuto.TowerManager.GetTowerByTypeAndName (TowerType.Antiair, "tuto2");
		var cannon = lvlTuto.TowerManager.GetTowerByTypeAndName (TowerType.Cannon, "tuto2");

		var pos = Camera.main.WorldToScreenPoint(antiAir.transform.position);
		lvlTuto.LevelCanvasManager.holdMoveImage.gameObject.SetActive (true);
		lvlTuto.LevelCanvasManager.holdMoveImage.rectTransform.position = pos;

		pos = Camera.main.WorldToScreenPoint(cannon.transform.position);
		lvlTuto.LevelCanvasManager.secondHoldMoveImage.rectTransform.position = pos;


	}
}
