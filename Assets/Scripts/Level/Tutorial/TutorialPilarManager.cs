using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPilarManager : TutorialOneManager
{
    public List<Transform> pilars;

    public bool CanClick { get; set; }


    int _minionCount;
    OverchargeEventManager _overchargeManager;

    protected override void AddListener()
    {
        lvl.MinionManager.OnNewMinionSpawned += NewMinionBuiltHandler;

        _overchargeManager = FindObjectOfType<OverchargeEventManager>();
    }

    protected override void AddSteps()
    {
        steps.Add(new StepBase());//do nothing
        steps.Add(new MoveCameraToPoint(this));
        steps.Add(new ShowExplainText(this, "pilartext_1",true));
        steps.Add(new ShowExplainText(this, "pilartext_2",false));
        steps.Add(new ShowExplainText(this, "pilartext_3",false));
        steps.Add(new StopPilarTutotial(this));
    }

    void NewMinionBuiltHandler(MinionType t)
    {
        _minionCount++;
        if(_minionCount == _overchargeManager.minionsAmount)
        {
            OnExecuteStep(null);
        }
    }

    void Update()
    {
        if (CanClick && Input.GetMouseButtonUp(0))
        {
            if (_currStep == steps.Count)
                return;

            OnExecuteStep(null);
        }
    }
}
