using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public HitAreaCollider forTankThree; //spawn skill coll

    LevelCanvasTutorial _lvlCanvasTuto;
    List<MinionType> _toAdd = new List<MinionType>();

    bool _stopTutorial;
    bool _canShowTheOtherMinions;
    bool _showSkillBtns;
    bool _disableMinionSpawn = true;
    bool _builtFirstMinion;
    int _runnerCount;
    bool _runnerMinionHasDead;
    int _doveCount;
    int _tankCount;

    protected override void Init()
    {
        _toAdd.Add(MinionType.Runner);
        base.Init();
        _minionManager.OnMinionWalkFinished += MinionWalkFinishedHandler;
        _minionManager.OnMinionDeath += MinionDeathHandler;
        _minionManager.OnMinionSkillSelected += MinionSkillSelectedHandler;

        _towerManager.HideAllTowers();

        _lvlCanvasTuto = FindObjectOfType<LevelCanvasTutorial>();

        _lvlCanvasTuto.EnableArrowByName("PressFirstBtn");
        

    }

    protected override void InitLevelCanvas()
    {
        if (_lvlCanvasManager == null)
            _lvlCanvasManager = FindObjectOfType<LevelCanvasManager>();


        foreach (var item in _toAdd)
        {
            foreach (var m in availableMinions)
            {
                if (item != m.minionType) continue;

                var minionStats = GameManager.MinionsLoader.GetStatByLevel(m.minionType, levelID);
                m.pointsValue = minionStats.pointsValue;
                _lvlCanvasManager.BuildAvailableMinionButton(m, true);

                if(item == MinionType.Dove)
                {
                    forDoveOne.enabled = true;
                    forDoveTwo.enabled = true;
                    forRunner.enabled = false;
                    forTankOne.enabled = false;
                    forTankTwo.enabled = false;
                    forTankThree.enabled = false;
                }
                else if(item == MinionType.Runner)
                {
                    forDoveOne.enabled = false;
                    forDoveTwo.enabled = false;
                    forTankOne.enabled = false;
                    forTankTwo.enabled = false;
                    forTankThree.enabled = false;
                    forRunner.enabled = true;
                }
                else if(item == MinionType.Tank)
                {
                    forDoveOne.enabled = false;
                    forDoveTwo.enabled = false;
                    forRunner.enabled = false;
                    forTankOne.enabled = true;
                    forTankTwo.enabled = true;
                    forTankThree.enabled = true;
                    objetives[objetives.Length - 1] = 3;
                }
            }
        }
        
        _lvlCanvasManager.ShowHideSkillButtons(_showSkillBtns);

        _lvlCanvasManager.level = this;
        _lvlCanvasManager.UpdateLevelTimer(levelTime);
        _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);
        UpdatePoints(0);
    }


    public override bool BuildMinion(MinionType t)
    {
        var result = base.BuildMinion(t);
        if (result)
        {
            /*if(!_stopTutorial)
            {*/
                _lvlCanvasTuto.DisableAllArrows();
            //}
            if (t == MinionType.Dove)
                _doveCount++;
            if (t == MinionType.Runner && _tankCount ==1)
                _runnerCount++;

            if (t == MinionType.Tank)
            {
                _tankCount++;
                Destroy(forRunner.gameObject);
                Destroy(forDoveOne.gameObject);
                Destroy(forDoveTwo.gameObject);
            }
            else if(_tankCount == 1)
            {
                if(_runnerCount == 3 && _doveCount == 2)
                {
                    Time.timeScale = 1;
                    _stopTutorial = true;
                    return result;
                }

                if (t == MinionType.Dove)
                {
                    _lvlCanvasTuto.EnableArrowByName("PressSecondBtn");
                }
                else if (t == MinionType.Runner)
                {
                    _lvlCanvasTuto.EnableArrowByName("PressThirdBtn");
                }
            }
        }

        return result;
    }

    public void OnPopupButtonPressed()
    {
        if(_runnerCount == 1)
        {
            _towerManager.ShowTowerByTypeAndName(TowerType.Cannon, "tuto1");
            _lvlCanvasManager.EnableMinionButtons(true);
            _livesRemoved = 0;
            _lvlCanvasManager.UpdateLevelLives(LivesRemoved, objetives[objetives.Length - 1]);
        }

        if (_runnerCount == 2 && _doveCount == 0)
        {
            _toAdd.Remove(MinionType.Runner);
            _toAdd.Add(MinionType.Dove);
            _towerManager.HideAllTowers();
            _towerManager.ShowTowerByTypeAndName(TowerType.Cannon, "tuto2");
            _towerManager.ShowTowerByTypeAndName(TowerType.Antiair, "tuto2");
            _lvlCanvasManager.DeleteMinionButtons();
            forDoveOne.OnTriggerEnterCallback += OnEnter;
            forDoveTwo.OnTriggerEnterCallback += OnEnter;
            forRunner.OnTriggerEnterCallback -= OnEnter;

            _livesRemoved = 0;
            InitLevelCanvas();
        }

        if(_doveCount == 1)
        {
            _toAdd = new List<MinionType>();
            _toAdd.Add(MinionType.Tank);
            _toAdd.Add(MinionType.Runner);
            _toAdd.Add(MinionType.Dove);//doing this so tank is the first button so i don't have to change tuto arrow's position.

            _towerManager.HideAllTowers();
            _towerManager.ShowTowerByTypeAndName(TowerType.Cannon, "tuto1");
            _towerManager.ShowTowerByTypeAndName(TowerType.Antiair, "tuto3");
            _towerManager.ShowTowerByTypeAndName(TowerType.Laser);

            forRunner.OnTriggerEnterCallback -= OnEnter;
            forDoveOne.OnTriggerEnterCallback -= OnEnter;
            forDoveTwo.OnTriggerEnterCallback -= OnEnter;
            forTankOne.OnTriggerEnterCallback += OnTankEnterOne;
            forTankTwo.OnTriggerEnterCallback += OnTankEnterTwo;
            forTankThree.OnTriggerEnterCallback += OnTankEnterTwo;

            _livesRemoved = 0;
            _lvlCanvasManager.DeleteMinionButtons();
            InitLevelCanvas();
        }

        if (!_stopTutorial)
            _lvlCanvasTuto.EnableArrowByName("PressFirstBtn");

        if (_runnerMinionHasDead)
        {
            _runnerMinionHasDead = false;
            forRunner.OnTriggerEnterCallback += OnEnter;
        }
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
            _runnerCount++;

            if (_runnerCount == 1)
            {
                _gameManager.popupManager.BuildOneButtonPopup(_lvlCanvasManager.transform
                    , "Well done!"
                    , "You arrived to the end of path."
                    , "Continue"
                    , "TutorialCero");
            }
            else if (_runnerCount == 2)
            {
                _gameManager.popupManager.BuildOneButtonPopup(_lvlCanvasManager.transform
                    , "Awesome!"
                    , "You know how to use the runner skill"
                    , "Continue"
                    , "TutorialCero");
            }
        }

        if(type == MinionType.Dove)
        {
            if(_doveCount == 1 )
                _gameManager.popupManager.BuildOneButtonPopup(_lvlCanvasManager.transform
                    , "Well done!"
                    , "You arrived to the end of path."
                    , "Continue"
                    , "TutorialCero");
        }


    }
    
    void MinionDeathHandler(MinionType type)
    {
        if(type == MinionType.Runner)
        {
            if(_runnerCount == 1)
                _runnerMinionHasDead = true;
        }

        _gameManager.popupManager.BuildOneButtonPopup(_lvlCanvasManager.transform
                , "Ups"
                , "Your minion has been killed. Try using his skill!"
                , "Try Again"
                , "TutorialCero");
    }

    void OnEnter(Collider col)
    {
        _lvlCanvasTuto.EnableArrowByName("PointingToRunnerSkill");
        var pos = Camera.main.WorldToScreenPoint(col.transform.position);
        _lvlCanvasTuto.SetArrowPosition(pos, "PointingToRunnerSkill");
        Time.timeScale = 0;
    }

    void OnTankEnterOne(Collider col)
    {
        if(col.GetComponent<Minion>().minionType == MinionType.Tank)
        {
            _lvlCanvasTuto.EnableArrowByName("PressSecondBtn");
            _lvlCanvasTuto.EnableArrowByName("PressThirdBtn");
            Time.timeScale = 0;
            forTankOne.OnTriggerEnterCallback -= OnTankEnterOne;

        }
    }

    void OnTankEnterTwo(Collider col)
    {
        if (col.GetComponent<Minion>().minionType == MinionType.Tank)
        {
            _lvlCanvasTuto.EnableArrowByName("PointingToRunnerSkill");
            var pos = Camera.main.WorldToScreenPoint(col.transform.position);
            _lvlCanvasTuto.SetArrowPosition(pos, "PointingToRunnerSkill");
            Time.timeScale = 0;
            
        }
    }

    void MinionSkillSelectedHandler(MinionType t)
    {
        _lvlCanvasTuto.DisableAllArrows();
        Time.timeScale = 1;
    }
}
