using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFirstPartOfTuto : StepBase 
{
	public StopFirstPartOfTuto(Level _lvl, TutorialOneManager tutoOne)
	{
		lvl = _lvl;
		tutoOneManager = tutoOne;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		Time.timeScale = 1;

		tutoOneManager.canvasTuto.DisableArrowByName("skill1");
		tutoOneManager.canvasTuto.DisableArrowByName("skill2");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("skillText", false);

		lvl.LevelCanvasManager.EnableMinionButtons (true);
	}
}
