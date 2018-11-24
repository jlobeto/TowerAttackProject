using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour
{
	public int minionSlots = 5;
    [HideInInspector]
    public Level level;
    [HideInInspector]
    public Text pointsText;
    public CanvasSkillLvlButton skillButtonPrefab;
    public Image levelPointBar;
    public Button minionSaleButtonPrefab;
    public Image eventWarning;
	public RectTransform swapTowerTutorial;
	public Image tapUpImage;
	public Image tapDownImage;
	public Image holdMoveImage;
	public Image secondHoldMoveImage;
	public List<MinionSaleButton> minionSaleButtons = new List<MinionSaleButton>();

    HorizontalLayoutGroup _skillsButtonPanel;
    /// <summary>
    /// Available minions (the level will fill this ones)
    /// </summary>
	HorizontalLayoutGroup _availablesPanel;
    HorizontalLayoutGroup _minionSkillPanel;
    //DragAndDropSystem _dragAndDropSystem;
    List<CanvasSkillLvlButton> _skillButtons = new List<CanvasSkillLvlButton>();
	CameraMovement _cameraMove;

    Image _levelTimerBG;
    Image _levelLivesBG;
    Text _levelTimer;
    Text _levelLives;
    Text _eventWarningText;
    bool _isAnyButtonDisabled;
	bool _isUpFinger;
	bool _stopTapAnimation;
	Canvas _thisCanvas;

    string _evtType;
    bool _eventWarningEnabled;
    float _eventWarningTime;

	bool _startTutorialHoldAnimation;
	Vector3 _holdImageTargetPosition;
	Vector3 _holdImageInitPosition;

    void Awake()
    {
		_thisCanvas = GetComponent<Canvas> ();
        var panels = GetComponentsInChildren<HorizontalLayoutGroup>();
        _skillsButtonPanel = panels.FirstOrDefault(i => i.tag == "LvlSkillPanel");
		_availablesPanel = panels.FirstOrDefault(i => i.tag == "AvailablesPanel");
		_minionSkillPanel = panels.FirstOrDefault(i => i.tag == "MinionSkillPanel");

        //_dragAndDropSystem = GetComponentInChildren<DragAndDropSystem>();
        //_timerBtn = GetComponentsInChildren<Button>().FirstOrDefault(i => i.tag == "BuildSquadTimer");
        //_timerText = _timerBtn.GetComponentInChildren<Text>();
        //_timerBtn.onClick.AddListener(() => OnTimerButtonClicked());
        pointsText = levelPointBar.transform.parent.GetComponentInChildren<Text>();
        foreach (Transform child in transform)
        {
            if (child.tag == "CanvasLvlTimer")
				_levelTimerBG = child.GetComponentInChildren<Image>();
            if(child.tag == "CanvasLvlLives")
				_levelLivesBG = child.GetComponentInChildren<Image>();
        }

        _levelTimer = _levelTimerBG.GetComponentInChildren<Text>();
        _levelLives = _levelLivesBG.GetComponentInChildren<Text>();

        _eventWarningText = eventWarning.GetComponentInChildren<Text>();
        eventWarning.gameObject.SetActive(false);
        _eventWarningText.enabled = false;

		tapUpImage.gameObject.SetActive (false);
		tapDownImage.gameObject.SetActive (false);

		_cameraMove = Camera.main.GetComponentInParent<CameraMovement> ();
    }

    void Update()
    {
        UpdateEventWarning();
		HoldDownAndMoveTutorialAnim();
    }

	public void EnableSwapTowerTutorial(Vector3 towerPos)
	{
		var pos = Camera.main.WorldToScreenPoint(towerPos);
		var timeShowingTuto = 1f;
		_cameraMove.StartSwapTutorial (pos, timeShowingTuto);
		Time.timeScale = 0;
		StartCoroutine (SwapTowerTutoTimer(timeShowingTuto, towerPos));
	}

	IEnumerator SwapTowerTutoTimer(float timeShowingTuto, Vector3 towerPos)
	{
		yield return new WaitForSecondsRealtime (timeShowingTuto);

		StartCoroutine (WaitToFinishSwap ());

		var pos = Camera.main.WorldToScreenPoint(towerPos);
		swapTowerTutorial.position = new Vector3(pos.x, pos.y + 35 , 0);
	}

	IEnumerator WaitToFinishSwap()
	{
		yield return new WaitForSecondsRealtime (6f);
		swapTowerTutorial.position = new Vector3(10000, 10000, 0);
		Time.timeScale = 1;
	}

    public void ShowHideAllUI(bool value)
    {
		_skillsButtonPanel.gameObject.SetActive( value );
		string tag = "";
		foreach (Transform child in transform)
		{
			tag = child.tag;
			if (tag == "CanvasLvlTimer" || tag == "CanvasLvlLives" || tag == "CanvasLvlPoints")
				child.gameObject.SetActive (value);
		}
    }

    public void EnableMinionButtons(bool value)
    {
        var btns = _availablesPanel.GetComponentsInChildren<Transform>().Where(i => i.GetComponent<Button>() != null);
        var realBtns = btns.Select(i => i.GetComponent<Button>());
        foreach (var item in realBtns)
        {
            item.interactable = value;
        }
    }

	public void EnableMinionSaleSpecific(bool value, MinionType type)
	{
		foreach (Transform item in _availablesPanel.transform) 
		{
			var b = item.GetComponent<MinionSaleButton> ();
			if (b.minionType == type)
				item.GetComponent<Button> ().interactable = value;
		}
	}

    public void DeleteMinionButtons()
    {
        var btns = _availablesPanel.GetComponentsInChildren<Transform>().Where(i => i.GetComponent<Button>() != null);
        var realBtns = btns.Select(i => i.GetComponent<Button>());
        foreach (var item in realBtns)
        {
            Destroy(item.gameObject);
        }
    }
    
    public void UpdateLevelTimer(float newTime)
    {
        var text = "Time: ";
		_levelTimer.text = (text + newTime.ToString("0.0")).ToUpper();
    }

    public void UpdateLevelLives(int newLive, int initLives)
    {
        _levelLives.text = newLive + "/" + initLives;
    }

	public void BuildMinionSlots(List<Minion> availableMinions, int lvlId, MinionsSkillManager minionSkillsManager, bool stayNotInteractuable = false)
    {   
		var slots = 0;
		foreach (var m in availableMinions) //need to get json data to show correct point value on spawn button 
		{
			if (m == null) {
				Debug.LogError("minion item in AvailableMinions List is null ");
				continue;
			}
			slots++;
			var minionStats = level.GameManager.MinionsLoader.GetStatByLevel (m.minionType, lvlId);
			m.pointsValue = minionStats.pointsValue;

			var btn = Instantiate<Button>(minionSaleButtonPrefab, _availablesPanel.transform);
			btn.GetComponentInChildren<Text>().text = m.minionType +" x"+ m.pointsValue;
			minionSaleButtons.Add (btn.GetComponent<MinionSaleButton> ());
			minionSaleButtons[minionSaleButtons.Count-1].minionType = m.minionType;
			minionSaleButtons[minionSaleButtons.Count-1].minionSkill = m.skillType;
			var t = m.minionType;
			var newBtn = btn;
			var cooldown = m.spawnCooldown;
			var fillImg = btn.GetComponentsInChildren<Image>();//Returns btn.image and its child.image(DONT KNOW WHY)
			btn.onClick.AddListener(() => OnBuyMinion(newBtn,fillImg[1], t, cooldown, stayNotInteractuable));

			SetMinionSkillButton (btn, m.skillType, !stayNotInteractuable, minionSkillsManager);
		}

		if (slots < minionSlots) 
		{
			var lefts = minionSlots - slots;
			BuildEmptySlots (lefts, _availablesPanel.transform);
		}
    }

	void BuildEmptySlots(int quantity, Transform parent)
	{
		for (int i = 0; i < quantity; i++) 
		{
			var btn = Instantiate<Button>(minionSaleButtonPrefab, parent);
			btn.interactable = false;
			btn.GetComponentInChildren<Text>().text = "";
			var fillImg = btn.GetComponentsInChildren<Image>()[1];//Returns btn.image and its child.image(DONT KNOW WHY)
			fillImg.fillAmount = 1;

			SetMinionSkillButton (btn, BaseMinionSkill.SkillType.None, false, null);
		}	
	}

	public void SetMinionSkillButton(Button baseBtn, BaseMinionSkill.SkillType skill, bool interactable, MinionsSkillManager minionSkillsManager)
	{
		var skillBtn = baseBtn.GetComponentInChildren<MinionSkillMouseDown> ();

		if (interactable) {
			skillBtn.InitButton (skill, minionSkillsManager.SkillButtonPressed);
			skillBtn.GetComponentInChildren<Text>().text = skill.ToString();
		}
			
		var fillImg = skillBtn.GetComponentsInChildren<Image>()[1];//Returns btn.image and its child.image(DONT KNOW WHY)
		fillImg.fillAmount = interactable ? 0 : 1;
		skillBtn.GetComponent<Button> ().interactable = interactable;
	}

	public MinionSaleButton GetSpecificMinionSaleBtn(MinionType t)
	{
		foreach (var item in minionSaleButtons) 
		{
			if (item.minionType == t)
				return item;
		}

		return null;
	}

    void OnBuyMinion(Button btn,Image fillImg, MinionType t, float cooldown, bool stayNotInteractuable)
    {
        if (!btn.interactable) return;
        
        var created = level.BuildMinion(t);
        if (!created) return;

        btn.interactable = false;

        if (!stayNotInteractuable)
        {
            fillImg.fillAmount = 1;
            StartCoroutine(BuyMinionFreeze(cooldown, fillImg, btn));
        }
    }


    IEnumerator BuyMinionFreeze(float totalTime, Image forground, Button btn)
    {
        var currTime = totalTime;
        while (currTime >= 0)
        {
            yield return new WaitForEndOfFrame();
            currTime -= Time.deltaTime;
            forground.fillAmount = (currTime / totalTime);
        }
        forground.fillAmount = 0;
        btn.interactable = true;
    }

    public void UpdateLevelPointBar(int newValue, int prevValue, int baseValue)
    {
        float n = (float)newValue;
        float b = (float)baseValue;
        levelPointBar.fillAmount = n / b;
        pointsText.text = newValue + " / " + baseValue;
        //Debug.Log(levelPointBar.fillAmount);
    }

    public void TriggerEventWarning(bool activate, float initTime , string eventType)
    {
        _eventWarningEnabled = activate;
        eventWarning.gameObject.SetActive(activate);
        _eventWarningText.enabled = activate;
        _eventWarningTime = initTime;

        if (eventType == "dust")
            eventType = "Static storm";
        _evtType = eventType;
    }
    void UpdateEventWarning()
    {
        if (!_eventWarningEnabled) return;

        _eventWarningTime -= Time.deltaTime;
        _eventWarningText.text = _evtType.ToUpper() + " IN: " + _eventWarningTime.ToString("0.0") + "''";
    }

    #region Skills Buttons
    public void CreateSkillButton(LevelSkill skill,Action onActivate, Action onDeactivate)
    {
        var lvlBtn = Instantiate<CanvasSkillLvlButton>(skillButtonPrefab, transform);
        lvlBtn.type = skill.skillType;
        lvlBtn.GetComponentInChildren<Text>().text = skill.stats.skillType + " " + skill.stats.useCountPerLevel+ "/" + skill.stats.useCountPerLevel;
        lvlBtn.button.onClick.AddListener(() => SkillButtonCallback(onActivate, onDeactivate, lvlBtn.GetInstanceID()));
        lvlBtn.transform.SetParent(_skillsButtonPanel.transform);
        _skillButtons.Add(lvlBtn);
    }

    void SkillButtonCallback(Action onActivate, Action onDeactivate, int goID)
    {
        if (!_isAnyButtonDisabled)
        {
            onActivate();
            DeactivateSkillButtons(goID);
        }
        else
        {
            onDeactivate();
            TryActivatingSkillButtons();
        }
    }

    /// <summary>
    /// Handles the visual stuff of the button of type 'type' ones the skill is executed.
    /// </summary>
    public void SkillExecutedVisualHandler(LevelSkillManager.SkillType type, bool interactable, int currentUses, int initUses)
    {
        var skillBtn = _skillButtons.FirstOrDefault(i => i.type == type);
        skillBtn.button.interactable = skillBtn.usable = interactable;
        skillBtn.button.GetComponentInChildren<Text>().text = type.ToString() + " " + (initUses-currentUses) + "/" + initUses;
        TryActivatingSkillButtons();
    }

    /// <summary>
    /// If button type is not exception and it is usable, it will activate it
    /// </summary>
    public void TryActivatingSkillButtons()
    {
        foreach (var item in _skillButtons)
        {
            if(item.usable)
                item.button.interactable = true;
        }

        _isAnyButtonDisabled = false;
    }

    public void ActivateSkillButtons()
    {
        foreach (var item in _skillButtons)
        {
            item.button.interactable = true;
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
                item.button.interactable = false;
            }
        }
    }
    #endregion
	int tapAnimationCount;
	public void StartTapAnimation(MinionType t, bool setPosition = false)
	{
		_stopTapAnimation = false;
		tapAnimationCount++;

		var btn = GetSpecificMinionSaleBtn (t).GetComponent<Image>();
		var anchor = btn.rectTransform.anchoredPosition;

		if (tapAnimationCount == 1 || setPosition)
		{
			tapUpImage.rectTransform.position = new Vector3(anchor.x + 80,  anchor.y + (_availablesPanel.transform.position.y * .2f));
			tapDownImage.rectTransform.position = new Vector3(anchor.x + 80,  anchor.y + (_availablesPanel.transform.position.y * .2f));
		}
	
		tapUpImage.gameObject.SetActive (true);

		StartCoroutine (OnTapChange());
	}

	IEnumerator OnTapChange()
	{
		yield return new WaitForSeconds (.5f);

		if (_stopTapAnimation) 
		{
			tapDownImage.gameObject.SetActive (false);
			tapUpImage.gameObject.SetActive (false);
		}
			
		tapDownImage.gameObject.SetActive (_isUpFinger);
		tapUpImage.gameObject.SetActive (!_isUpFinger);

		_isUpFinger = !_isUpFinger;

		if (!_stopTapAnimation)
			StartCoroutine (OnTapChange ());
		else 
		{
			tapDownImage.gameObject.SetActive (false);
			tapUpImage.gameObject.SetActive (false);
		}
	}

	public void StopTapAnimation()
	{
		_stopTapAnimation = true;
		_isUpFinger = true;
	}

	public void StartSkillTapAnimation (MinionType t, Vector3 toPosition)
	{
		holdMoveImage.gameObject.SetActive (true);

		var btn = GetSpecificMinionSaleBtn (t).GetComponent<Image>();

		var anchor = tapUpImage.rectTransform.position;
		time = 0;
		holdMoveImage.rectTransform.position = new Vector3(anchor.x ,  anchor.y + _availablesPanel.transform.position.y + 230);
		_startTutorialHoldAnimation = true;
		_holdImageTargetPosition = toPosition;
		_holdImageInitPosition = holdMoveImage.rectTransform.position;
	}

	float time;
	/// <summary>
	/// Holds down and move tutorial animation (minion skill selector.
	/// </summary>
	void HoldDownAndMoveTutorialAnim ()
	{
		if (!_startTutorialHoldAnimation)
			return;
		
		time += (Time.unscaledDeltaTime * 0.5f);
		holdMoveImage.rectTransform.position = Vector3.Lerp (_holdImageInitPosition, _holdImageTargetPosition, time);
		if (Mathf.Abs (Vector3.Distance (holdMoveImage.rectTransform.position, _holdImageTargetPosition)) < 0.5f) 
		{
			holdMoveImage.rectTransform.position = _holdImageInitPosition;
			time = 0;
		}
	}

	public void StopHoldDownMoveAnim()
	{
		_startTutorialHoldAnimation = false;
		holdMoveImage.gameObject.SetActive (false);
	}
}
