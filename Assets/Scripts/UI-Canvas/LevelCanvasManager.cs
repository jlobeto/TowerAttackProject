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
    public Button skillButtonPrefab;
    public Image levelPointBar;
    public Button minionSaleButtonPrefab;

    HorizontalLayoutGroup _skillsButtonPanel;
    HorizontalLayoutGroup _availablesPanel;
    DragAndDropSystem _dragAndDropSystem;
    List<Button> _skillButtons = new List<Button>();
    Button _timerBtn;
    Text _timerText;
    float _buildSquadTimer;
    float _levelTimer;
    bool _isAnyButtonDisabled;
    bool _readyToDiscountTimer;
    bool _buildTimerHasEnded;
    bool _levelTimerEnded;
    bool _pointBarLerpAnim;

    void Awake()
    {
        
        var panels = GetComponentsInChildren<HorizontalLayoutGroup>();
        _skillsButtonPanel = panels.FirstOrDefault(i => i.tag == "LvlSkillPanel");
        _availablesPanel = panels.FirstOrDefault(i => i.tag == "AvailablesPanel");
        _dragAndDropSystem = GetComponentInChildren<DragAndDropSystem>();
        _timerBtn = GetComponentsInChildren<Button>().FirstOrDefault(i => i.tag == "BuildSquadTimer");
        _timerText = _timerBtn.GetComponentInChildren<Text>();
        _timerBtn.onClick.AddListener(() => OnTimerButtonClicked());
        pointsText = levelPointBar.transform.parent.GetComponentInChildren<Text>();

        
    }

    void Update() {
        DiscountTimer();
    }

    void OnTimerButtonClicked()
    {
        if (_buildTimerHasEnded)
        {
            _timerBtn.onClick.RemoveAllListeners();
            return;
        }

        BuildSquadTimeStops();
        _timerBtn.onClick.RemoveAllListeners();
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
            BuildSquadTimeStops();
        }

        LevelEndCheckerByTime();
    }

    void BuildSquadTimeStops()
    {
        if (!_buildTimerHasEnded)
        {
            Destroy(_dragAndDropSystem.gameObject);
            _dragAndDropSystem = null;
            _buildSquadTimer = _levelTimer;
            level.startMinionSpawning = true;
            foreach (var item in _skillButtons)
                item.gameObject.SetActive(true);
        }

        _buildTimerHasEnded = true;
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
        //_buildSquadTimer = squad;
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
        var created = level.BuildMinion(t, !_buildTimerHasEnded);
        if(created && _dragAndDropSystem != null)
            _dragAndDropSystem.AddSlot(t);
    }

    public void MinionOrderUpdated(int from, int to)
    {
        level.MinionOrderHasChanged(from, to);
    }

    public void MinionDeleted(int index)
    {
        level.MinionDeletedByDandD(index);
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
