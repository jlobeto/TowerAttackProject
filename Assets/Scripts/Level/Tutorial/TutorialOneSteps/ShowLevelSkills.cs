using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLevelSkills : StepBase {

	public ShowLevelSkills(Level _lvl, TutorialOneManager tutoOne)
	{
		lvl = _lvl;
		tutoOneManager = tutoOne;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{

		tutoOneManager.canvasTuto.EnableArrowByName ("skill1");
		tutoOneManager.canvasTuto.EnableArrowByName ("skill2");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("skillText", true);

		tutoOneManager.canvasTuto.DisableArrowByName("Lives");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("livesText", false);

		tutoOneManager.StartTimer ();
	}
}