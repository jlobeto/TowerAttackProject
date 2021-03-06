﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinionSkillMouseDown : MonoBehaviour , IPointerDownHandler, IPointerUpHandler
{
	public Action<BaseMinionSkill.SkillType> OnSkillButtonDown = delegate {};
    public Image img;
    public Color pressedColor;
    [Range(0.5f, 1f)]
    public float pressedDownscale = 1;

    BaseMinionSkill.SkillType _myType;

	bool _init;
    Color _originalColor;
    Vector2 _originalScale;
    Image _buttonBackground;

    void Awake()
    {
        _buttonBackground = GetComponent<Image>();
    }

	public void InitButton(BaseMinionSkill.SkillType myType, Action<BaseMinionSkill.SkillType> callback, string parentName, GameManager gm)
	{
		_init = true;
		_myType = myType;
        _originalColor = img.color;
        _originalScale = img.rectTransform.sizeDelta;
        
        
        _buttonBackground.enabled = true;

        OnSkillButtonDown += callback;
        name = "skill_button_" + parentName[parentName.Length - 1];

        if (myType == BaseMinionSkill.SkillType.None)
        {
            img.gameObject.SetActive(false);
            _buttonBackground.enabled = false;
        }
        else 
        {
            img.sprite = gm.LoadedAssets.GetSpriteByName(GameplayUtils.GetMinionTypeBySkill(_myType).ToString() + "_skill");
            img.gameObject.SetActive(true);
        }

    }

    public void SetDisabled()
    {
        img.gameObject.SetActive(false);
        _buttonBackground.enabled = false;
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

        img.color = pressedColor;
        img.rectTransform.sizeDelta = new Vector2(img.rectTransform.sizeDelta.x * pressedDownscale, img.rectTransform.sizeDelta.y * pressedDownscale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_init)
            return;

        img.color = _originalColor;
        img.rectTransform.sizeDelta = _originalScale;
    }
}
