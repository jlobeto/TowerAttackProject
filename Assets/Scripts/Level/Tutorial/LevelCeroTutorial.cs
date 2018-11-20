using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// This is the level tutorial for level 0.
/// </summary>
public class LevelCeroTutorial : Level
{

    public HitAreaCollider forRunner;
    public HitAreaCollider forDoveOne;
    public HitAreaCollider forDoveTwo;
    public HitAreaCollider forTankOne; // spawn more minions
    public HitAreaCollider forTankTwo; //show skill range
	public bool tankTutoStarted;

    LevelCanvasTutorial _lvlCanvasTuto;
	[HideInInspector]public List<MinionType> addMinionButton = new List<MinionType>();
	TutorialCeroManager _tutoManager;

    bool _stopTutorial;
    bool _canShowTheOtherMinions;
    bool _builtFirstMinion;
    int _runnerCount;
    bool _runnerMinionHasDead;
    int _doveCount;
    int _tankCount;
	bool _hasViewedFirstTowerRange;//if user saw only one tower range ps (dove part)
	TowerType _otherTower;
	int _spawnedAfterTank;

	public LevelCanvasTutorial LevelCanvasTutorial{get{ return _lvlCanvasTuto;}}

    protected override void Init()
    {
		_lvlCanvasTuto = FindObjectOfType<LevelCanvasTutorial>();

        base.Init();
        _minionManager.OnMinionWalkFinished += MinionWalkFinishedHandler;
        _minionManager.OnMinionDeath += MinionDeathHandler;
        _minionManager.OnMinionSkillSelected += MinionSkillSelectedHandler;
		_towerManager.OnClickTower += TowerClickedHandler;
        _towerManager.HideAllTowers();

    }

    protected override void InitLevelCanvas()
    {
        if (_lvlCanvasManager == null)
            _lvlCanvasManager = FindObjectOfType<LevelCanvasManager>();
        
        _lvlCanvasManager.ShowHideAllUI(false);

        _lvlCanvasManager.level = this;

		//Add Runner;
		//ExecuteTutorialStep (gameObject);
		//Activate Arrow
		ExecuteTutorialStep(_lvlCanvasTuto.gameObject);
    }


    public override bool BuildMinion(MinionType t)
    {
        var result = base.BuildMinion(t);

		if (t == MinionType.Runner) 
		{
			_runnerCount++;
			if (!tankTutoStarted)
				ExecuteTutorialStep (gameObject);
			else
				ExecuteTutorialStep (null);
			
		}
		else if (t == MinionType.Dove)
		{
			_doveCount++;
			if (_doveCount == 1) {
				_lvlCanvasManager.StopTapAnimation ();
			}
		} 
		else if (t == MinionType.Tank) 
		{
			_tankCount++;
			_livesRemoved = 0;
			if (_tankCount == 1)
				LevelCanvasManager.StopTapAnimation ();
		}

		if (tankTutoStarted && t != MinionType.Tank) 
		{
			//ExecuteTutorialStep (MinionManager.GetMinion(t).gameObject);

			if (t == MinionType.Dove)
			{
				_spawnedAfterTank++;
			}
			else if (t == MinionType.Runner)
			{
				_spawnedAfterTank++;
			}

			if (_spawnedAfterTank == 2)
			{
				Time.timeScale = 1;
				_stopTutorial = true;
			}
		}

        return result;
    }

    public void OnPopupButtonPressed()
    {
		//if first runner ends path.
		//if second runner ends path > dove part triggered.
		//Dove has arrived to end.
		ExecuteTutorialStep (gameObject);

    }

    protected override void GoalCompletedHandler()
    {
        if (_stopTutorial)
            base.GoalCompletedHandler();
    }

    void MinionWalkFinishedHandler(MinionType type)
    {
        if (type == MinionType.Runner)
        {
			if(!tankTutoStarted)
				ExecuteTutorialStep (gameObject);
        }

        if(type == MinionType.Dove)
        {
			if (_doveCount == 1) 
			{
				ExecuteTutorialStep (null);
			}
        }
	}
    
    void MinionDeathHandler(MinionType type)
    {
        if(type == MinionType.Runner)
        {
            if(_runnerCount == 2)
				ExecuteTutorialStep (gameObject);
        }
    }

    public void OnRunnerColEnter(Collider col)
    {
		forRunner.OnTriggerEnterCallback -= OnRunnerColEnter;
		
		ExecuteTutorialStep (col.gameObject);
    }

	public void OnDoveColEnter(Collider col)
	{
		ExecuteTutorialStep (col.gameObject);
	}

    public void OnTankEnterOne(Collider col)
    {
        if(col.GetComponent<Minion>().minionType == MinionType.Tank)
        {
			ExecuteTutorialStep (null);
        }
    }

    public void OnTankEnterTwo(Collider col)
    {
        if (col.GetComponent<Minion>().minionType == MinionType.Tank)
        {
			ExecuteTutorialStep (null);
        }
    }

    void MinionSkillSelectedHandler(MinionType t)
    {
		if(_runnerCount == 3 && _doveCount == 0)
		{
			LevelCanvasManager.StopHoldDownMoveAnim ();
			var m = _minionManager.GetMinion (MinionType.Runner);
			m.SetWalk (true);
		}

		if(_doveCount == 1)
		{
			LevelCanvasManager.StopHoldDownMoveAnim ();
			var m = _minionManager.GetMinion (MinionType.Dove);
			m.SetWalk (true);
			LevelCanvasManager.SetMinionSkillButton (LevelCanvasManager.GetSpecificMinionSaleBtn(m.minionType).GetComponent<Button>()
				, m.skillType,false, MinionSkillManager);
		}

		if(_tankCount == 1)
		{
			LevelCanvasManager.StopHoldDownMoveAnim ();
			var m = _minionManager.GetMinion (MinionType.Tank);
			var minions = MinionManager.GetMinions(GetAllMinions);

			foreach (var item in minions) 
			{
				item.SetWalk (true);
			}

			LevelCanvasManager.SetMinionSkillButton (LevelCanvasManager.GetSpecificMinionSaleBtn(m.minionType).GetComponent<Button>()
				, m.skillType,false, MinionSkillManager);
		}
			
    }

	void TowerClickedHandler(TowerType t)
	{
		if(t == TowerType.Antiair)
			_lvlCanvasManager.holdMoveImage.rectTransform.position = new Vector3 (-1500,-10000);
		else
			_lvlCanvasManager.secondHoldMoveImage.rectTransform.position = new Vector3 (-1500,-10000);
		
		if (_hasViewedFirstTowerRange && _otherTower != t)
		{
			ExecuteTutorialStep (gameObject);
			_towerManager.OnClickTower -= TowerClickedHandler;
		}

		_hasViewedFirstTowerRange = true;
		_otherTower = t;
	}

	bool GetAllMinions(Minion m)
	{
		return true;
	}
}
