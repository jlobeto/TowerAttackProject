using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCanvasManager : MonoBehaviour
{
    public Level level;
    public Button skillButtonPrefab;
    public Image levelPointBar;
    public Button minionSaleButtonPrefab;

    HorizontalLayoutGroup _skillsButtonPanel;
    HorizontalLayoutGroup _availablesPanel;
    List<Button> _skillButtons = new List<Button>();
    Button _timerBtn;
    Text _timerText;
    float _buildSquadTimer;
    float _levelTimer;
    bool _isAnyButtonDisabled;
    bool _readyToDiscountTimer;
    bool _buildTimerHasEnded;
    bool _levelTimerEnded;

    void Awake()
    {
        var panels = GetComponentsInChildren<HorizontalLayoutGroup>();
        _skillsButtonPanel = panels.FirstOrDefault(i => i.tag == "LvlSkillPanel");
        _availablesPanel = panels.FirstOrDefault(i => i.tag == "AvailablesPanel");

        _timerBtn = GetComponentsInChildren<Button>().FirstOrDefault(i => i.tag == "BuildSquadTimer");
        _timerText = _timerBtn.GetComponentInChildren<Text>();
    }

    void Update() {
        DiscountTimer();
    }

    void DiscountTimer()
    {
        if (!_readyToDiscountTimer) return;

        if(!_levelTimerEnded)
            _buildSquadTimer -= Time.deltaTime;

        var text = _buildTimerHasEnded ? "Time ! " : "Squad ! ";
        _timerText.text = text + _buildSquadTimer.ToString("0.00");
        if (_buildSquadTimer < 0)
        {
            if (!_buildTimerHasEnded)
            {
                _buildSquadTimer = _levelTimer;
                level.startMinionSpawning = true;
                foreach (var item in _skillButtons)
                    item.gameObject.SetActive(true);
            }
                
            _buildTimerHasEnded = true;
        }

        LevelEndCheckerByTime();
    }

    void LevelEndCheckerByTime()
    {
        if (!_buildTimerHasEnded) return;
        if (_buildSquadTimer >= 0 || _levelTimerEnded) return;

        Debug.Log("LEVEL HAS ENDED ------ ");
        _levelTimerEnded = true;
        _buildSquadTimer = 0;
    }

    public void SetBuildSquadTimer(float squad, float level)
    {
        _buildSquadTimer = squad;
        _levelTimer = level;
        _readyToDiscountTimer = true;
    }

    public void BuildAvailableMinionsButtons(List<MinionType> types)
    {
        foreach (var item in types)
        {
            var btn = Instantiate<Button>(minionSaleButtonPrefab, _availablesPanel.transform);
            btn.GetComponentInChildren<Text>().text = item.ToString();
            var t = item;//fucking lazyness
            btn.onClick.AddListener(() => OnBuyMinion(t));
        }
    }

    void OnBuyMinion(MinionType t)
    {
        level.BuildMinion(t, !_buildTimerHasEnded);
    }

    public void UpdateLevelPointBar(int newValue, int baseValue)
    {
        float n = (float)newValue;
        float b = (float)baseValue;
        levelPointBar.fillAmount = n / b;
        //Debug.Log(levelPointBar.fillAmount);
    }

    #region Skills Buttons
    public void CreateSkillButton(string name, Action onActivate, Action onDeactivate)
    {
        var btn = Instantiate<Button>(skillButtonPrefab, transform);
        btn.GetComponentInChildren<Text>().text = name;
        btn.onClick.AddListener(() => SkillButtonCallback(onActivate, onDeactivate, btn.GetInstanceID()));
        btn.transform.SetParent(_skillsButtonPanel.transform);
        btn.gameObject.SetActive(false);
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
