using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowElectroshockText : StepBase
{
    TutorialElectroshockManager _tutoMan;
    string _textName;

    public ShowElectroshockText(string textName, TutorialElectroshockManager t)
    {
        _textName = textName;
        _tutoMan = t;
    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        _tutoMan.canvasTuto.DisableTexts();
        _tutoMan.canvasTuto.EnableDisableTextByName(_textName, true);
    }
}
