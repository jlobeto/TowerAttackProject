using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelPopup : AcceptPopup
{
    public GameObject starsContainer;
    public GameObject LoseButtonContainer;
    public Sprite starWin;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        
    }

    public override void InitPopup(string parameters)
    {
        var split = parameters.Split(',');
        foreach (var item in split)
        {

        }
    }
}
