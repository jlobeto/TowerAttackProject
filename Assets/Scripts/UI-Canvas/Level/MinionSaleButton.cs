using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionSaleButton : MonoBehaviour 
{
	public MinionType minionType;
	public BaseMinionSkill.SkillType minionSkill;
    public Image charImg;

    bool _interactuable;
    Button _btn;

    private void Awake()
    {
        _btn = GetComponent<Button>();
    }

    private void Start()
    {
        if (minionSkill != BaseMinionSkill.SkillType.None)
        {
            var s = Resources.Load<Sprite>("UIMinionsPictures/" + minionType.ToString() + "/" + minionType.ToString());
            charImg.sprite = s;
            _interactuable = true;
        }
        else
        {
            charImg.gameObject.SetActive(false);
            _interactuable = false;
        }
    }

    public void SetInteractability(bool force = false, bool value = false)
    {
        if (force)
            _btn.interactable = value;
        else
        {
            _btn.interactable = _interactuable;
        }

    }
}
