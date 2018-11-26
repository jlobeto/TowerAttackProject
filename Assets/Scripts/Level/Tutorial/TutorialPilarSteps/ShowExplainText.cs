using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowExplainText : StepBase
{
    TutorialPilarManager tutoPilarManager;
    string _textName;
    bool _isFirst;

    public ShowExplainText(TutorialPilarManager tutoOne, string pilarTextName, bool isFirstShown)
    {
        tutoPilarManager = tutoOne;
        _textName = pilarTextName;
        _isFirst = isFirstShown;
    }

    public override void ExecuteStep(GameObject gameObject = null)
    {
        if(_isFirst)
        {
            tutoPilarManager.CanClick = true;
            tutoPilarManager.canvasTuto.EnableArrowByName("arrow");
            var pos = Camera.main.WorldToScreenPoint(tutoPilarManager.pilars[0].transform.position);
            tutoPilarManager.canvasTuto.SetArrowParentPosition(pos, "arrow", 65);
        }

        tutoPilarManager.canvasTuto.DisableTexts();

        tutoPilarManager.canvasTuto.EnableDisableTextByName(_textName, true);

    }
}