using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoveTutorialStart : StepBase
{

	public DoveTutorialStart(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.TowerManager.HideAllTowers();
		lvlTuto.TowerManager.ShowTowerByTypeAndName(TowerType.Cannon, "tuto2");
		lvlTuto.TowerManager.ShowTowerByTypeAndName(TowerType.Antiair, "tuto2");

		lvlTuto.LevelCanvasManager.DeleteMinionButtons();

		lvlTuto.addMinionButton.Remove(MinionType.Runner);
		lvlTuto.addMinionButton.Add (MinionType.Dove);
		foreach (var item in lvlTuto.addMinionButton)
		{
			foreach (var m in lvlTuto.availableMinions)
			{
				if (item != m.minionType) continue;

				var minionStats = lvlTuto.GameManager.MinionsLoader.GetStatByLevel(m.minionType, 0);
				m.pointsValue = minionStats.pointsValue;
				lvlTuto.LevelCanvasManager.BuildAvailableMinionButton(m, true);
			}
		}

		lvlTuto.forDoveOne.OnTriggerEnterCallback += lvlTuto.OnDoveColEnter;
		lvlTuto.forDoveTwo.OnTriggerEnterCallback += lvlTuto.OnDoveColEnter;
	}
}
