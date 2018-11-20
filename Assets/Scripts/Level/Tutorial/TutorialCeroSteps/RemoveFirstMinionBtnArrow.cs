using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFirstMinionBtnArrow : StepBase{

	public RemoveFirstMinionBtnArrow(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject)
	{
		lvlTuto.LevelCanvasManager.StopTapAnimation ();
	}
}
