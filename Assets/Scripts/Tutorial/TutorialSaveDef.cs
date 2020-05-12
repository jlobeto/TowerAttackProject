using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TutorialSaveDef
{
    public string lastTutorialGroupId;
    public bool didShowFirstTutoPopup;
    public bool didAgreeWithDoFirstTutorial;
    public List<string> tutorialPhasesCompleted;//if the phase is completed or has been canceled it goes inside this list

    public TutorialSaveDef()
    {
        tutorialPhasesCompleted = new List<string>();
    }
}
