using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankEnterFirstCollider : StepBase
{
	public TankEnterFirstCollider(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.forTankOne.OnTriggerEnterCallback -= lvlTuto.OnTankEnterOne;

		var m = lvlTuto.MinionManager.GetMinion (MinionType.Tank);

		var minions = lvlTuto.MinionManager.GetMinions(GetAllMinions);

		foreach (var item in minions) 
		{
			item.SetWalk (false);
		}

		lvlTuto.LevelCanvasManager.SetMinionSkillButton (lvlTuto.LevelCanvasManager.GetSpecificMinionSaleBtn(m.minionType).GetComponent<Button>()
			, m.skillType,true, lvlTuto.MinionSkillManager);

		var pos = Camera.main.WorldToScreenPoint(m.transform.position);
		lvlTuto.LevelCanvasManager.StartSkillTapAnimation (MinionType.Tank, pos);
		lvlTuto.LevelCanvasManager.StopTapAnimation ();
	}


	bool GetAllMinions(Minion m)
	{
		return true;
	}
}
