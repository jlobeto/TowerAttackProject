using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBase : IStep
{
	public LevelCeroTutorial lvlTuto;
	public TutorialOneManager tutoOneManager;
	public Level lvl;

	#region IStep implementation

	public virtual void ExecuteStep (GameObject gameObject = null)
	{
		//throw new System.NotImplementedException ();
	}


	#endregion
}
