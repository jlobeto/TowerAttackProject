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
    public Text pointsText;
    public CanvasSkillLvlButton skillButtonPrefab;
    public Image levelPointBar;
    public Button minionSaleButtonPrefab;
    public Image eventWarning;
    [HideInInspector] public List<MinionSaleButton> minionSaleButtons = new List<MinionSaleButton>();
    public LiveRayEffect liveRaySprite;
    public Sprite starOnSprite;

    HorizontalLayoutGroup _skillsButtonPanel;
    /// <summary>
    /// Available minions (the level will fill this ones)
    /// </summary>
	HorizontalLayoutGroup _availablesPanel;
    HorizontalLayoutGroup _minionSkillPanel;
    //DragAndDropSystem _dragAndDropSystem;
    List<CanvasSkillLvlButton> _skillButtons = new List<CanvasSkillLvlButton>();
	CameraMovement _cameraMove;

    Image _levelTimerFillBar;
    Image _levelLivesFillBar;
    Text _levelTimer;
    //Text _levelLives;
    Text _eventWarningText;
    bool _isAnyButtonDisabled;
	Canvas _thisCanvas;

    string _evtType;
    bool _eventWarningEnabled;
    float _eventWarningTime;

	bool _startTutorialHoldAnimation;
    int _currentStarsOn;

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
        foreach (Transform child in transform)
        {
            if (child.tag == "CanvasLvlTimer")
            {
                _levelTimerFillBar = child.GetComponentsInChildren<Image>().FirstOrDefault(i => i.type == Image.Type.Filled);
                _levelTimerFillBar.fillAmount = 1;
            }
				
        }

        _levelTimer = _levelTimerFillBar.transform.parent.GetComponentInChildren<Text>();
        //_levelLives = _levelLivesBG.GetComponentInChildren<Text>();

        _eventWarningText = eventWarning.GetComponentInChildren<Text>();
        eventWarning.gameObject.SetActive(false);
        _eventWarningText.enabled = false;

		_cameraMove = Camera.main.GetComponentInParent<CameraMovement> ();
    }

    void Update()
    {
        UpdateEventWarning();
    }

	public void EnableSwapTowerTutorial(Vector3 towerPos)
	{
		var pos = Camera.main.WorldToScreenPoint(towerPos);
		var timeShowingTuto = 1f;
		_cameraMove.StartCameraMoveForTutorial (pos, timeShowingTuto);
		Time.timeScale = 0;
		StartCoroutine (SwapTowerTutoTimer(timeShowingTuto, towerPos));
	}

	IEnumerator SwapTowerTutoTimer(float timeShowingTuto, Vector3 towerPos)
	{
		yield return new WaitForSecondsRealtime (timeShowingTuto);

		StartCoroutine (WaitToFinishSwap ());

		var pos = Camera.main.WorldToScreenPoint(towerPos);
		//swapTowerTutorial.position = new Vector3(pos.x, pos.y + 35 , 0);
	}

	IEnumerator WaitToFinishSwap()
	{
		yield return new WaitForSecondsRealtime (6f);
		//swapTowerTutorial.position = new Vector3(10000, 10000, 0);
        _cameraMove.SetCameraMovement(true);
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

    public void EnableMinionButtons(bool value, bool forceValue = false)
    {
        var btns = _availablesPanel.GetComponentsInChildren<MinionSaleButton>().ToList();

        foreach (var item in btns)
        {
            item.SetInteractability(forceValue, value);
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
    
    public void UpdateLevelTimer(float newTime, float initTime, bool isInfinite = false)
    {
        var text = "Time: ";
        if(isInfinite)
        {
            _levelTimer.text = text + "∞";
            return;
        }

		_levelTimer.text = (text + newTime.ToString("0.0")).ToUpper();
        _levelTimerFillBar.fillAmount = newTime / initTime;
    }

    public void UpdateLevelLives(int newLive, int initLives)
    {
        if (level.levelID == 0) return;

        if(_levelLivesFillBar == null )
        {
            SetLivesUI();
        }
        else
            SendRaySpriteToLiveBar();

        //_levelLives.text = newLive + "/" + initLives;
        float newL = (float)newLive;
        float initL = (float)initLives;
        _levelLivesFillBar.fillAmount = newL / initL;

        UpdateStarsUI();
    }

    void UpdateStarsUI()
    {
        var starsWon = level.GetCurrentStarsWinning();

        if (_currentStarsOn == starsWon) return;

        _currentStarsOn = starsWon;
        var parent = _levelLivesFillBar.rectTransform.parent.GetComponent<RectTransform>();
        var stars = parent.GetComponentsInChildren<Image>();
        var min = stars.FirstOrDefault(i => i.name.Contains("min"));
        var mid = stars.FirstOrDefault(i => i.name.Contains("mid"));
        var max = stars.FirstOrDefault(i => i.name.Contains("max"));

        if (_currentStarsOn == 1)
            min.sprite = starOnSprite;

        if(_currentStarsOn == 2)
            mid.sprite = starOnSprite;

        if(_currentStarsOn == 3)
            max.sprite = starOnSprite;

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
			var minionStats = level.GameManager.MinionsJsonLoader.GetStatByLevel (m.minionType, lvlId);
			m.pointsValue = minionStats.pointsValue;

			var btn = Instantiate<Button>(minionSaleButtonPrefab, _availablesPanel.transform);
			btn.GetComponentInChildren<Text>().text = m.pointsValue.ToString();
			minionSaleButtons.Add (btn.GetComponent<MinionSaleButton> ());
			minionSaleButtons[minionSaleButtons.Count-1].minionType = m.minionType;
			minionSaleButtons[minionSaleButtons.Count-1].minionSkill = m.skillType;
			var t = m.minionType;
			var newBtn = btn;
			var cooldown = m.spawnCooldown;
			var fillImg = minionSaleButtons[minionSaleButtons.Count - 1].offButtonImg;
			btn.onClick.AddListener(() => OnBuyMinion(newBtn,fillImg, t, cooldown, stayNotInteractuable));

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

			SetMinionSkillButton (btn, BaseMinionSkill.SkillType.None, false, null);
		}	
	}

	public void SetMinionSkillButton(Button baseBtn, BaseMinionSkill.SkillType skill, bool interactable, MinionsSkillManager minionSkillsManager)
	{
		var skillBtn = baseBtn.GetComponentInChildren<MinionSkillMouseDown> ();

		if (interactable)
        {
			skillBtn.InitButton (skill, minionSkillsManager.SkillButtonPressed);
			//skillBtn.GetComponentInChildren<Text>().text = skill.ToString();
		}
        else
        {
            skillBtn.img.gameObject.SetActive(false);
        }

        /*var fillImg = skillBtn.GetComponentsInChildren<Image>()[1];//Returns btn.image and its child.image(DONT KNOW WHY)
		fillImg.fillAmount = interactable ? 0 : 1;*/
        skillBtn.GetComponent<Button> ().interactable = interactable;
        skillBtn.SetOnPointerDown(interactable);
	}

    public void EnableDisableMinionSkillButtons(bool value)
    {
        foreach (var item in minionSaleButtons)
        {
            SetMinionSkillButton(item.GetComponent<Button>(), item.minionSkill, value, level.MinionSkillManager);
        }
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
    
    /// <summary>
    /// Don't touch this, is awfull but it works xD
    /// </summary>
    void SetLivesUI()
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "CanvasLvlLives") continue;

            var imgs = child.GetComponentsInChildren<Image>();
            _levelLivesFillBar = imgs.FirstOrDefault(i => i.type == Image.Type.Filled);
            break;
        }

        var parent = _levelLivesFillBar.rectTransform.parent.GetComponent<RectTransform>();
        var width = parent.sizeDelta.x;
        Image min, mid, max;

        var stars = parent.GetComponentsInChildren<Image>();
        min = stars.FirstOrDefault(i => i.name.Contains("min"));
        mid = stars.FirstOrDefault(i => i.name.Contains("mid"));
        max = stars.FirstOrDefault(i => i.name.Contains("max"));

        float livesToWin = (float)level.objetives[2];

        var yPosForAll = min.rectTransform.position.y;

        var percent = (float)level.objetives[0] / livesToWin;
        var posX_1 = percent * width ; 
        
        min.rectTransform.localPosition = new Vector3(posX_1, 0);

        percent = (float)level.objetives[1] / livesToWin;
        var posX_2 = percent * width ;
        mid.rectTransform.localPosition = new Vector3(posX_2 , 0);

        max.rectTransform.localPosition = new Vector3(width-27, 0);
        
    }

    
    void SendRaySpriteToLiveBar()
    {
        var spawnWorldPos = level.levelPortal.raySpawnPoint.position;
        var spawnScreenPos = Camera.main.WorldToScreenPoint(spawnWorldPos);
        var ray = Instantiate<LiveRayEffect>(liveRaySprite, transform);
        ray.Init(spawnScreenPos, _levelLivesFillBar.transform.parent.position);
    }
}
