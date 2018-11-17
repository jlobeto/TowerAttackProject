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

		lvlTuto.forDoveOne.OnTriggerEnterCallback += lvlTuto.OnDoveColEnter;
		lvlTuto.forDoveTwo.OnTriggerEnterCallback += lvlTuto.OnDoveColEnter;
	}
}
