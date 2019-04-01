using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPopup : BasePopup
{
    public override void DisplayPopup()
    {
        if (isShowing) return;

        _rect.position = new Vector3(_rect.parent.position.x, _rect.parent.position.y);
        gameObject.SetActive(true);
        base.DisplayPopup();
    }

    public override void OkButtonPressed()
    {
        if (!isShowing) return;

        ExecuteFunctions(FunctionTypes.ok);
        gameObject.SetActive(false);
        isShowing = false;
    }
}
