using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSkillLvlButton : MonoBehaviour
{
    Button _button;

    public LevelSkillManager.SkillType type;
    public Button button { get { return _button; } }
    public bool usable = true;

    void Awake()
    {
        usable = true;
        _button = GetComponent<Button>();    
    }

}
