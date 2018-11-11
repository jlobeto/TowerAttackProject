using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//step 2 - 
public class AddFirstMinionBtnArrow : StepBase
{
	public AddFirstMinionBtnArrow(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.LevelCanvasTutorial.EnableArrowByName("PressFirstBtn");		
	}
}
