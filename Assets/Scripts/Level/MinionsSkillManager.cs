using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsSkillManager : MonoBehaviour 
{

	Level _lvl;
	SpriteRenderer _rangeSprite;
	bool _pressDown;
	float _rangeRadius = 5;
	RaycastHit hit;
	Ray ray;
	BaseMinionSkill.SkillType _typeToSelect;
	List<Minion> _selectedOnes;
	IEnumerable<Minion> _theOnes;
	Dictionary<BaseMinionSkill.SkillType, Color> _skillToColor = new Dictionary<BaseMinionSkill.SkillType, Color>();

	void Start () 
	{
		_rangeSprite = Resources.Load("Level/MinionSkillSelector" , typeof(SpriteRenderer)) as SpriteRenderer;
		_rangeSprite = Instantiate<SpriteRenderer> (_rangeSprite, new Vector3(1000,1000,1000), Quaternion.identity);
		_rangeSprite.transform.localScale = new Vector3 (_rangeRadius, _rangeRadius,1);
		_rangeSprite.transform.Rotate (90, 0, 0);

		_skillToColor.Add (BaseMinionSkill.SkillType.SpeedBoost, new Color(214/255, 2/255, 10/255));
		_skillToColor.Add (BaseMinionSkill.SkillType.HitShield, new Color(0, 97/255, 250));
		_skillToColor.Add (BaseMinionSkill.SkillType.ChangeTarget, new Color(212/255, 4/255, 240/255));
		_skillToColor.Add (BaseMinionSkill.SkillType.GiveHealth, new Color(0, 1, 96/255));
		_skillToColor.Add (BaseMinionSkill.SkillType.SmokeBomb, new Color(178/255, 1, 0));//light green
		_skillToColor.Add (BaseMinionSkill.SkillType.WarScreamer, new Color(240/255, 170/255, 0));//light green
	}
	

	void Update () 
	{
		if (!_pressDown)
			return;

		if (Input.touchCount == 0)
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		else 
			ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);

		if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Floor")))
		{
			_rangeSprite.transform.position = hit.point;
		}

		if(Input.GetMouseButtonUp(0))//desktop and mobile
		{
			_pressDown = false;

			//using global variables to not create a new reference each buttonUp
			_selectedOnes = _lvl.GameOBjectSelector.GetMinionsSelection (_rangeSprite.transform.position, _rangeRadius);
			_theOnes = _selectedOnes.Where (i => i.skillType == _typeToSelect);

			foreach (var m in _theOnes)
				m.ActivateSelfSkill ();

			_rangeSprite.transform.position = new Vector3 (1000, 1000, 1000);
		}

	}

	public void Init(Level lvl)
	{
		_lvl = lvl;
	}

	public void SkillButtonPressed(BaseMinionSkill.SkillType skill)
	{
		if (_pressDown)
			return;
		
		var m = _lvl.availableMinions.First (i => i.skillType == skill);
		if (m == null)
			return;
		_typeToSelect = skill;
		_pressDown = true;
		_rangeSprite.color = _skillToColor [skill];
		Debug.Log (skill);
	}


	private void OnDrawGizmos()
	{
		/*if (_pressDown) {
			Gizmos.DrawWireSphere (_rangeSprite.transform.position, _rangeRadius);
			var point2 = new Vector3 (_rangeSprite.transform.position.x, _rangeSprite.transform.position.y + 6, _rangeSprite.transform.position.z);
			Gizmos.DrawWireSphere (point2, _rangeRadius);
		}*/
	}
}
