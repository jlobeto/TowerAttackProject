using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour
{
    public Button skillButtonPrefab;

    HorizontalLayoutGroup _skillsButtonPanel;
    List<Button> _skillButtons = new List<Button>();
    bool _isAnyButtonDisabled;

    
	void Start () {
        _skillsButtonPanel = GetComponentInChildren<HorizontalLayoutGroup>();
    }
	
	void Update () {
		
	}

    #region Skills Buttons
    public void CreateSkillButton(string name, Action onActivate, Action onDeactivate)
    {
        var btn = Instantiate<Button>(skillButtonPrefab, transform);
        btn.GetComponentInChildren<Text>().text = name;
        btn.onClick.AddListener(() => SkillButtonCallback(onActivate, onDeactivate, btn.GetInstanceID()));
        btn.transform.SetParent(_skillsButtonPanel.transform);
        _skillButtons.Add(btn);
    }


    void SkillButtonCallback(Action onClick, Action onDeactivate, int goID)
    {
        if (!_isAnyButtonDisabled)
        {
            onClick();
            DeactivateSkillButtons(goID);
        }
        else
        {
            onDeactivate();
            ActivateSkillButtons();
        }
    }

    public void ActivateSkillButtons()
    {
        foreach (var item in _skillButtons)
        {
            item.interactable = true;
        }

        _isAnyButtonDisabled = false;
    }

    void DeactivateSkillButtons(int activatedOne)
    {
        foreach (var item in _skillButtons)
        {
            if (item.GetInstanceID() != activatedOne)
            {
                _isAnyButtonDisabled = true;
                item.interactable = false;
            }
                
        }
    }
    #endregion
}
