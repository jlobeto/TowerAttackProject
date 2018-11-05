using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour
{
    [HideInInspector]
    public Level level;
    [HideInInspector]
    public Text pointsText;
    public CanvasSkillLvlButton skillButtonPrefab;
    public Image levelPointBar;
    public Button minionSaleButtonPrefab;
    public Image eventWarning;

    HorizontalLayoutGroup _skillsButtonPanel;
    /// <summary>
    /// Available minions (the level will fill this ones)
    /// </summary>
    HorizontalLayoutGroup _availablesPanel;
    //DragAndDropSystem _dragAndDropSystem;
    List<CanvasSkillLvlButton> _skillButtons = new List<CanvasSkillLvlButton>();
    Image _levelTimerBG;
    Image _levelLivesBG;
    Text _levelTimer;
    Text _levelLives;
    Text _eventWarningText;
    bool _isAnyButtonDisabled;
    bool _pointBarLerpAnim;

    string _evtType;
    bool _eventWarningEnabled;
    float _eventWarningTime;

    void Awake()
    {
        var panels = GetComponentsInChildren<HorizontalLayoutGroup>();
        _skillsButtonPanel = panels.FirstOrDefault(i => i.tag == "LvlSkillPanel");
        _availablesPanel = panels.FirstOrDefault(i => i.tag == "AvailablesPanel");
        //_dragAndDropSystem = GetComponentInChildren<DragAndDropSystem>();
        //_timerBtn = GetComponentsInChildren<Button>().FirstOrDefault(i => i.tag == "BuildSquadTimer");
        //_timerText = _timerBtn.GetComponentInChildren<Text>();
        //_timerBtn.onClick.AddListener(() => OnTimerButtonClicked());
        pointsText = levelPointBar.transform.parent.GetComponentInChildren<Text>();
        foreach (Transform child in transform)
        {
            if (child.tag == "CanvasLvlTimer")
                _levelTimerBG = child.GetComponent<Image>();
            if(child.tag == "CanvasLvlLives")
                _levelLivesBG = child.GetComponent<Image>();
        }

        _levelTimer = _levelTimerBG.GetComponentInChildren<Text>();
        _levelLives = _levelLivesBG.GetComponentInChildren<Text>();

        _eventWarningText = eventWarning.GetComponentInChildren<Text>();
        eventWarning.enabled = false;
        _eventWarningText.enabled = false;
    }

    void Update()
    {
        UpdateEventWarning();
    }

    public void ShowHideSkillButtons(bool value)
    {
        _skillsButtonPanel.enabled = value;
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
        var text = "Level Time: ";
        _levelTimer.text = text + newTime.ToString("0.00") + " ";
    }

    public void UpdateLevelLives(int newLive, int initLives)
    {
        _levelLives.text = newLive + " / " + initLives;
    }

    public void BuildAvailableMinionButton(Minion m, bool stayNotInteractuable = false)
    {        
        var btn = Instantiate<Button>(minionSaleButtonPrefab, _availablesPanel.transform);
        btn.GetComponentInChildren<Text>().text = m.minionType +" x"+ m.pointsValue;
        var t = m.minionType;
        var newBtn = btn;
        var cooldown = m.spawnCooldown;
        var fillImg = btn.GetComponentsInChildren<Image>();//Returns btn.image and its child.image(DONT KNOW WHY)
        btn.onClick.AddListener(() => OnBuyMinion(newBtn,fillImg[1], t, cooldown, stayNotInteractuable));
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
        _pointBarLerpAnim = true;
        float n = (float)newValue;
        float b = (float)baseValue;
        levelPointBar.fillAmount = n / b;
        pointsText.text = newValue + " / " + baseValue;
        //Debug.Log(levelPointBar.fillAmount);
    }

    public void TriggerEventWarning(bool activate, float initTime , string eventType)
    {
        _eventWarningEnabled = activate;
        eventWarning.enabled = activate;
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

    #region CommentedFunctions
    /*void OnTimerButtonClicked()
    {
    if (_buildTimerHasEnded)
    {
        _timerBtn.onClick.RemoveAllListeners();
        return;
    }

    BuildSquadTimeStops();
    _timerBtn.onClick.RemoveAllListeners();
    }*/
    /*
    public void MinionOrderUpdated(int from, int to)
    {
        level.MinionOrderHasChanged(from, to);
    }

    public void MinionDeleted(int index)
    {
        level.MinionDeletedByDandD(index);
    }
    */
    #endregion

}
