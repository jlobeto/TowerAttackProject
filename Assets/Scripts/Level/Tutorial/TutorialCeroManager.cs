using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCeroManager : MonoBehaviour 
{
	protected List<StepBase> steps = new List<StepBase>();

	LevelCeroTutorial _level;
	int _currStep;

	protected virtual void Awake () 
	{
		AddListener ();
		AddSteps ();
	}

	protected virtual void AddListener()
	{
		_level = FindObjectOfType<LevelCeroTutorial> ();
		_level.ExecuteTutorialStep += OnExecuteStep;
	}

	/// <summary>
	/// Raises the next step event. 
	/// </summary>
	/// <param name="gameObject">Game object that will be affected by the next step.</param>
	protected void OnExecuteStep(GameObject gameObject = null)
	{
		if (steps.Count == _currStep) {
			Debug.Log ("there is not another step in the queue.");
			return;
		}
		
		steps [_currStep].ExecuteStep (gameObject);
		_currStep++;
	}

	protected virtual void AddSteps()
	{
		//first part
		steps.Add (new AddRunner(_level));
		steps.Add (new AddFirstMinionBtnArrow(_level));
		steps.Add (new RemoveFirstMinionBtnArrow(_level));//when user touch the button.
		steps.Add (new ShowFirstRunnerPopup(_level)); //when runner ends the path (first time)
		steps.Add (new AddCannonTower(_level));
		steps.Add (new RemoveFirstMinionBtnArrow(_level));
		steps.Add (new RunnerHasBeenKilled(_level));//show popup + add ontrigger listener.
		steps.Add (new RunnerUseTheSkill(_level));//when runner dies, unlock the button and point that button. 
		steps.Add (new RemoveFirstMinionBtnArrow(_level));//when user touch the button.
		steps.Add (new OnRunnerEnterToUseSkill(_level));
		steps.Add (new ShowRunnerSkillUserPopup(_level));

		//second part (dove)
		steps.Add (new DoveTutorialStart(_level));
		steps.Add (new DoveEnterCol(_level));
		steps.Add (new ShowDovePointingArrow(_level));
		steps.Add (new ShowDovePointingArrow(_level));
		steps.Add (new OnDoveArrivedToEnd(_level));

		//third part (tank) on last popup button clicked.
		steps.Add (new OnTankTutoStart(_level));
		steps.Add (new AddTapOnRunnerInTankTuto(_level, true)); //runner
		steps.Add (new AddTapOnRunnerInTankTuto(_level, false));// dove button
		steps.Add (new TankEnterFirstCollider(_level)); //actually is the second collider (did not have time to change class name).
		//steps.Add (new SpawnedMinionAfterTank(_level));
		//steps.Add (new SpawnedMinionAfterTank(_level));

	}
}
