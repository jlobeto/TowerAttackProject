using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTimer : StepBase
{
	public ShowTimer(TutorialOneManager tutoOne)
	{
		tutoOneManager = tutoOne;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		tutoOneManager.canvasTuto.DisableArrowByName("PointsBar");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("pointsText", false);

		tutoOneManager.canvasTuto.EnableArrowByName("levelTime");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("hurryUp", true);

		tutoOneManager.StartTimer ();
	}
}