using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1 step
public class AddRunner : StepBase
{
	public AddRunner(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.addMinionButton.Add (MinionType.Runner);
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
	}
}
