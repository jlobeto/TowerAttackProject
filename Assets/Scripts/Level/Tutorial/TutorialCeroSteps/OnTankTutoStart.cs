using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTankTutoStart : StepBase
{
	public OnTankTutoStart(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.TowerManager.HideAllTowers();
		lvlTuto.TowerManager.ShowTowerByTypeAndName(TowerType.Cannon, "tuto1");
		lvlTuto.TowerManager.ShowTowerByTypeAndName(TowerType.Antiair, "tuto3");
		lvlTuto.TowerManager.ShowTowerByTypeAndName(TowerType.Laser);

		lvlTuto.LevelCanvasManager.DeleteMinionButtons();

		lvlTuto.addMinionButton.Remove (MinionType.Dove);
		lvlTuto.addMinionButton.Add(MinionType.Tank);
		lvlTuto.addMinionButton.Add(MinionType.Runner);
		lvlTuto.addMinionButton.Add(MinionType.Dove);

		var list = new List<Minion> ();
		foreach (var item in lvlTuto.addMinionButton)
		{
			foreach (var m in lvlTuto.availableMinions)
			{
				if (item != m.minionType) continue;
				list.Add (m);
			}
		}

		lvlTuto.LevelCanvasManager.BuildMinionSlots(list, lvlTuto.levelID,lvlTuto.MinionSkillManager, true);

		lvlTuto.forTankOne.OnTriggerEnterCallback += lvlTuto.OnTankEnterOne;
		lvlTuto.forTankTwo.OnTriggerEnterCallback += lvlTuto.OnTankEnterTwo;
		lvlTuto.forTankThree.OnTriggerEnterCallback += lvlTuto.OnTankEnterTwo;

		lvlTuto.objetives [0] = 3;

		lvlTuto.tankTutoStarted = true;

		lvlTuto.LevelCanvasTutorial.EnableArrowByName("PressFirstBtn");
	}
}
