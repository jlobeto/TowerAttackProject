using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionSaleButton : MonoBehaviour 
{
	public MinionType minionType;
	public BaseMinionSkill.SkillType minionSkill;
    public Image charImg;
    public Image offButtonImg;

    bool _interactuable;
    Button _btn;
    Image _onButtonImg;

    private void Awake()
    {
        _btn = GetComponent<Button>();

    }

    private void Start()
    {
        
    }

    public void Init(GameManager gm) 
    {
        if (minionSkill != BaseMinionSkill.SkillType.None)
        {
            var s = gm.LoadedAssets.GetSpriteByName(minionType.ToString());
            charImg.sprite = s;
            _interactuable = true;
        }
        else
        {
            charImg.gameObject.SetActive(false);
            offButtonImg.fillAmount = 1;
            _interactuable = false;
        }
    }

    public void SetInteractability(bool force = false, bool value = false)
    {
        if (force)
        {
            _btn.interactable = value;
            offButtonImg.fillAmount = value ? 0 : 1;
        }
        else
        {
            _btn.interactable = _interactuable;
            offButtonImg.fillAmount = _interactuable ? 0 : 1;
        }

    }
}
