using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLives : StepBase {

	public ShowLives(Level _lvl, TutorialOneManager tutoOne)
	{
		lvl = _lvl;
		tutoOneManager = tutoOne;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		
		tutoOneManager.canvasTuto.EnableArrowByName ("Lives");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("livesText", true);

		tutoOneManager.canvasTuto.DisableArrowByName("levelTime");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("hurryUp", false);

		tutoOneManager.StartTimer ();
	}
}
