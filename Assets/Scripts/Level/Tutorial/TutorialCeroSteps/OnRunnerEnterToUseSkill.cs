using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnRunnerEnterToUseSkill : StepBase {


	public OnRunnerEnterToUseSkill(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		var runnerPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		//Time.timeScale = 0;

		var m = lvlTuto.MinionManager.GetMinion (MinionType.Runner);
		m.SetWalk (false);

		var saleBtn = lvlTuto.LevelCanvasManager.GetSpecificMinionSaleBtn(MinionType.Runner);
		lvlTuto.LevelCanvasManager.SetMinionSkillButton(saleBtn.GetComponent<Button>(), saleBtn.minionSkill, true, lvlTuto.MinionSkillManager);
		lvlTuto.LevelCanvasManager.StartSkillTapAnimation(MinionType.Runner, runnerPos);
	}
}
