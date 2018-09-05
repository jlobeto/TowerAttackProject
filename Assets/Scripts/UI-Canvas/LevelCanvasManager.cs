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
    /// <summary>
    /// Available minions (the level will fill this ones)
    /// </summary>
    HorizontalLayoutGroup _availablesPanel;
    //DragAndDropSystem _dragAndDropSystem;
    List<Button> _skillButtons = new List<Button>();
    Image _levelTimerBG;
    Image _levelLivesBG;
    Text _levelTimer;
    Text _levelLives;
    bool _isAnyButtonDisabled;
    bool _pointBarLerpAnim;

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
        
    }

    void Update()
    {

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
        var created = level.BuildMinion(t);
        /*For Drag&Drop system.
         * if(created && _dragAndDropSystem != null)
            _dragAndDropSystem.AddSlot(t);*/
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
        //btn.gameObject.SetActive(false);
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
