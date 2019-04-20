using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinionSkillMouseDown : MonoBehaviour , IPointerDownHandler
{
	public Action<BaseMinionSkill.SkillType> OnSkillButtonDown = delegate {};
    public Image img;

    BaseMinionSkill.SkillType _myType;

	bool _init;
	public void InitButton(BaseMinionSkill.SkillType myType, Action<BaseMinionSkill.SkillType> callback)
	{
		_init = true;
		_myType = myType;
		OnSkillButtonDown += callback;

        if (myType == BaseMinionSkill.SkillType.None)
            img.gameObject.SetActive(false);
        else 
        {
            img.sprite = Resources.Load<Sprite>("UIMinionsPictures/" + GameplayUtils.GetMinionTypeBySkill(_myType).ToString() + "/skill"); 
        }

    }

    public void SetOnPointerDown(bool value)
    {
        _init = value;
    }

	//Do this when the mouse is clicked over the selectable object this script is attached to.
	public void OnPointerDown(PointerEventData eventData)
	{
		if (!_init)
			return;
		
		OnSkillButtonDown (_myType);
	}
}
