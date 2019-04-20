using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPoints : StepBase
{
	public ShowPoints(Level _lvl, TutorialOneManager tutoOne)
	{
		lvl = _lvl;
		tutoOneManager = tutoOne;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
        Debug.Log("tutorial");
		Time.timeScale = 0;
		lvl.LevelCanvasManager.EnableMinionButtons (false);
		tutoOneManager.canvasTuto.EnableArrowByName ("PointsBar");
		tutoOneManager.canvasTuto.EnableDisableTextByName ("pointsText", true);
	}
}