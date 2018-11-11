using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnterFirstCollider : StepBase
{
	public TankEnterFirstCollider(LevelCeroTutorial tuto)
	{
		lvlTuto = tuto;
	}

	public override void ExecuteStep (GameObject gameObject = null)
	{
		lvlTuto.LevelCanvasTutorial.EnableArrowByName("PressSecondBtn");
		lvlTuto.LevelCanvasTutorial.EnableArrowByName("PressThirdBtn");
		Time.timeScale = 0;
		lvlTuto.forTankOne.OnTriggerEnterCallback -= lvlTuto.OnTankEnterOne;
	}
}
