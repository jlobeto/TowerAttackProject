using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDovePointingArrow : StepBase 
{
	public ShowDovePointingArrow(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		//Time.timeScale = 0;
		var m = lvlTuto.MinionManager.GetMinion (MinionType.Dove);
		m.SetWalk (false);

		lvlTuto.LevelCanvasManager.SetMinionSkillButton (lvlTuto.LevelCanvasManager.GetSpecificMinionSaleBtn(m.minionType).GetComponent<Button>()
			, m.skillType,true, lvlTuto.MinionSkillManager);

		var pos = Camera.main.WorldToScreenPoint(m.transform.position);
		lvlTuto.LevelCanvasManager.StartSkillTapAnimation (MinionType.Dove, pos);
	}
}
