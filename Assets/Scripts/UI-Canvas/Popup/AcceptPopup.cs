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
        Time.timeScale = 1;//TODO:: // SACAR ESTO A LA MIERDA
        var lvl = FindObjectOfType<Level>();
        lvl.LevelCanvasManager.EnableMinionButtons(true);
        lvl.LevelCanvasManager.EnableDisableMinionSkillButtons(true);
        ExecuteFunctions(FunctionTypes.cancel);
        OnClosePopup();
    }
}
