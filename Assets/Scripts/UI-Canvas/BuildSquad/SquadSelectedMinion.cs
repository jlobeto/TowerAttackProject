using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadSelectedMinion : MonoBehaviour
{
    const string UNSELECTED = "Unselected";

    public Button button;
    public Image minionPic;
    public Text text;
    public Action<string, bool, bool, MinionType> onMinionClick = delegate { };
    public MinionType minionType = MinionType.Runner;

    bool _isEmpty = true;

    void Start()
    {
        minionPic.enabled = false;
        text.text = UNSELECTED;
    }

    
    void Update()
    {
        
    }

    public void SetMinion(MinionType type)
    {

    }

}
