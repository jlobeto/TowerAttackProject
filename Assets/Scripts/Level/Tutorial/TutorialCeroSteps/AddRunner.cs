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
	}
}
