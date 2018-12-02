using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionArrows : StepBase
{
    TutorialElectroshockManager _tutoMan;
    string _textName;

    public RepositionArrows(string textName, TutorialElectroshockManager t)
    {
        _textName = textName;
        _tutoMan = t;
    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        _tutoMan.CanClick = true;
        _tutoMan.canvasTuto.EnableArrowByName("arrow_1");
        var pos = Camera.main.WorldToScreenPoint(_tutoMan.elbows[0].position);
        _tutoMan.canvasTuto.SetArrowParentPosition(pos, "arrow_1", 10);

        _tutoMan.canvasTuto.EnableArrowByName("arrow_2");
        pos = Camera.main.WorldToScreenPoint(_tutoMan.elbows[1].position);
        _tutoMan.canvasTuto.SetArrowParentPosition(pos, "arrow_2", 10);

        _tutoMan.canvasTuto.DisableTexts();

        _tutoMan.canvasTuto.EnableDisableTextByName(_textName, true);

    }
}