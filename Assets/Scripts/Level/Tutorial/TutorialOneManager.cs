using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOneManager : TutorialCeroManager
{

	public Level lvl;
	public LevelCanvasTutorial canvasTuto;
	int _minionDeathCount;

	protected override void Awake () 
	{
		lvl.ExecuteTutorialStep += OnExecuteStep;//added here because on Start() ocurred after the first ExecuteTutorialStep;
		AddSteps ();
	}

	void Start() 
	{
		AddListener ();

	}

	public void StartTimer()
	{
		StartCoroutine ( Timer());
	}

	protected override void AddListener ()
	{
		
		lvl.MinionManager.OnMinionDeath += MinionDeathHandler;
	}

	protected override void AddSteps ()
	{
		steps.Add (new ShowPoints(lvl, this));
		steps.Add (new ShowTimer(this));
		steps.Add (new ShowLives(lvl, this));
		steps.Add (new ShowLevelSkills(lvl, this));
		steps.Add (new StopFirstPartOfTuto(lvl, this));
	}

	IEnumerator Timer()
	{
		yield return new WaitForSecondsRealtime (5);
		OnExecuteStep (null);
	}

	void MinionDeathHandler(MinionType t)
	{
		_minionDeathCount++;
	}
}
