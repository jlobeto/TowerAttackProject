using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialElectroshockManager : TutorialOneManager
{
    public List<Transform> elbows;
    public Transform electroshockManager;

    public bool CanClick { get; set; }
    int _minionCount;

    protected override void AddListener()
    {

    }

    protected override void AddSteps()
    {
        steps.Add(new ShowFirstText("tower_text_1", this));
        steps.Add(new ShowElectroshockText("tower_text_2", this));
        steps.Add(new RepositionArrows("tower_text_3", this));
        steps.Add(new ShowElectroshockText("tower_text_4", this));
        steps.Add(new ShowElectroshockText("tower_text_5", this));
        steps.Add(new HideArrows("tower_text_6", this));
        steps.Add(new ShowElectroshockText("tower_text_7", this));
        steps.Add(new StopElectroshockTuto(this));

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
