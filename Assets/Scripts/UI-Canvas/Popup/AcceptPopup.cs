using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This popup has two buttons, one for accept and the other for cancel (AKA 'X')
/// </summary>
public class AcceptPopup : BasePopup
{
    public Button closeButton;

    protected override void Start()
    {
        closeButton.onClick.AddListener(() => CloseButton());
    }

    protected virtual void CloseButton()
    {
        ExecuteFunctions(FunctionTypes.cancel);
        OnClosePopup();
    }
}
