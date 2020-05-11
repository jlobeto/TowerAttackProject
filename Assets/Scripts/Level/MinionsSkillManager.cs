using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsSkillManager : MonoBehaviour 
{
    /// <summary>
    /// Used when the user triggers a skills after pressing skill button AND the range is affected
    /// by any minion.
    /// </summary>
    public Action OnSkillTriggered = delegate { };

	Level _lvl;
	SpriteRenderer _rangeSprite;
	ParticleSystem _ps;
	bool _pressDown;
	float _rangeRadius = 5;
	RaycastHit hit;
	Ray ray;
	BaseMinionSkill.SkillType _typeToSelect;
	List<Minion> _selectedOnes;
	IEnumerable<Minion> _theOnes;
	Dictionary<BaseMinionSkill.SkillType, Color> _skillToColor = new Dictionary<BaseMinionSkill.SkillType, Color>();
	CameraMovement _cameraMovement;
    Camera _mainCamera;

	void Start () 
	{
        _mainCamera = Camera.main;
        _cameraMovement = _mainCamera.GetComponentInParent<CameraMovement> ();

		_rangeSprite = Resources.Load("Level/MinionSkillSelector" , typeof(SpriteRenderer)) as SpriteRenderer;
		_rangeSprite = Instantiate<SpriteRenderer> (_rangeSprite, new Vector3(1000,1000,1000), Quaternion.identity);
		_rangeSprite.transform.localScale = new Vector3 (_rangeRadius, _rangeRadius,1);
		_rangeSprite.transform.Rotate (90, 0, 0);

		_ps = _rangeSprite.GetComponentInChildren<ParticleSystem> ();

		_skillToColor.Add (BaseMinionSkill.SkillType.SpeedBoost, new Color(1, 0, 0));
		_skillToColor.Add (BaseMinionSkill.SkillType.HitShield, new Color(0, 97f/255, 250));
		_skillToColor.Add (BaseMinionSkill.SkillType.ChangeTarget, new Color(212f/255, 4f/255, 240f/255));
		_skillToColor.Add (BaseMinionSkill.SkillType.GiveHealth, Color.green);
		_skillToColor.Add (BaseMinionSkill.SkillType.SmokeBomb, new Color(163f/255, 219f/255, 94f/255));//light green
		_skillToColor.Add (BaseMinionSkill.SkillType.WarScreamer, new Color(240f/255, 170f/255, 0));//light green
	}
	

	void Update ()
	{
		if (!_pressDown)
			return;

		if (Input.touchCount == 0)
			ray = _mainCamera.ScreenPointToRay (Input.mousePosition);
		else 
			ray = _mainCamera.ScreenPointToRay (Input.GetTouch(0).position);

		if (Physics.Raycast(ray, out hit, 1000, 1 << LayerMask.NameToLayer("Floor")))
		{
            _rangeSprite.transform.position = hit.point + _cameraMovement.transform.forward * 5;
		}

		if(Input.GetMouseButtonUp(0))//desktop and mobile
		{
			_pressDown = false;
			_cameraMovement.SetCameraMovement(true);

			//using global variables to not create a new reference each buttonUp
			_selectedOnes = _lvl.GameObjectSelector.GetMinionsSelection (_rangeSprite.transform.position, _rangeRadius);
			_theOnes = _selectedOnes.Where (i => i.skillType == _typeToSelect);

            
            foreach (var m in _theOnes)
            {
                //don't change the execution order of this foreach.
                if(!m.IsMainSkillLockedOrActive())
                    OnSkillTriggered();

                m.ActivateSelfSkill();
            }
				

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

		_cameraMovement.SetCameraMovement(false);

		_rangeSprite.color = _skillToColor [skill];
		var color = _ps.colorOverLifetime;
		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(_skillToColor [skill], 0.0f), new GradientColorKey(_skillToColor [skill], 1.0f) }
					, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );

		color.color = grad;
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
