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

    public bool IsEmpty { get { return _isEmpty; } }

    bool _isEmpty = true;



    void Awake()
    {
        minionPic.enabled = false;
        text.text = UNSELECTED;
    }

    
    void Update()
    {
        
    }

    public void SetMinion(MinionType type)
    {
        text.text = type.ToString();

        minionPic.enabled = true;
        minionPic.sprite = Resources.Load<Sprite>("UIMinionsPictures/" + text.text + "/" + text.text);

        _isEmpty = false;
        minionType = type;
    }

}
