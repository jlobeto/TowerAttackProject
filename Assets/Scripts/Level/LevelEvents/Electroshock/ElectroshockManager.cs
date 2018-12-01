using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ElectroshockManager : MonoBehaviour
{
    [Header("Time that user has to deactivate the effect")]
    public float timeToDeactivate = 5f;
    [Header("Duration of the effect on the path")]
    public float effectTime = 7f;
    [Header("Amount of 'elbows' the user can interact with")]
    public int availableToPress = 2;
    [Header("Amount of skills triggered successfully that are needed to rise event.")]
    public int amountOfSkillTriggered = 3;

    public List<ElectroshockElbow> elbows;

    Level _lvl;
    MeshRenderer[] _chargeLevels;
    int _currentChargeLvlIndex;
    int _amountOfSkillsAux;
    int _availableToPressAux;
    Color _activateFeedbackColor = Color.red;//new Color(0, 0.91f, 0.61f);
    bool _canTurnOffElbow;
    bool _isActive;//when the user has triggered the event and false when the electroshock finish;
    float _currentPercentOn;//for charges lvls feedback;
    float _percentOfEachChargeLvl;//for charges lvls feedback;

    void Start ()
    {
        _chargeLevels = GetComponentsInChildren<MeshRenderer>().Where(i => i.name.Contains("Charge lvl")).ToArray();
        _lvl = FindObjectOfType<Level>();
        _lvl.MinionSkillManager.OnSkillTriggered += SkillSelectedHandler;
        _amountOfSkillsAux = amountOfSkillTriggered;
        _availableToPressAux = availableToPress;

        _percentOfEachChargeLvl = (float)_chargeLevels.Length / amountOfSkillTriggered;

    }


    void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!_canTurnOffElbow) return;

            var selected = _lvl.GameObjectSelector.SelectGameObject(LayerMask.NameToLayer("ElectroshockElbow"));
            if (selected == null) return;

            foreach (var item in elbows)
            {
                if (item.gameObject.GetInstanceID() != selected.GetInstanceID()) continue;

                if(!item.IsDisabledByUser)
                {
                    item.DisableEvent();
                    _availableToPressAux--;
                    if (_availableToPressAux == 0)
                    {
                        ActivateElectroshock();
                    }
                }
            }
        }
    }

    public void SetChargeLevelFeedback()
    {
        if(_chargeLevels.Length > _currentChargeLvlIndex)
        {
            var mat = _chargeLevels[_currentChargeLvlIndex].material;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", _activateFeedbackColor);

            _currentChargeLvlIndex++;

            //if current index is before the last one, call again to turn on the above ball
            if (_currentChargeLvlIndex == _chargeLevels.Length - 1)
                SetChargeLevelFeedback();
        }
    }

    public void DeactivateAllChargeLvlFeedback()
    {
        Material mat;
        for (int i = 0; i < _chargeLevels.Length; i++)
        {
            mat = _chargeLevels[i].material;
            mat.SetColor("_EmissionColor", Color.black);
        }
    }

    void SkillSelectedHandler()
    {
        if (_isActive) return;

        _amountOfSkillsAux--;
        _currentPercentOn += _percentOfEachChargeLvl;
        var currLevel = _amountOfSkillsAux - amountOfSkillTriggered;

        if (_currentPercentOn >= 1)
        {
            int count = (int)_currentPercentOn;
            for (int i = 0; i < _currentPercentOn; i++)
                SetChargeLevelFeedback();

            _currentPercentOn = 0;
        }

        if(_amountOfSkillsAux == 0)
        {
            _isActive = true;
            _canTurnOffElbow = true;
            _amountOfSkillsAux = amountOfSkillTriggered;
            foreach (var item in elbows)
            {
                item.RiceEvent();
            }
            _currentPercentOn = 0;
            StartCoroutine(OnWaitUntilActivateEvent());
        }
    }

    void ActivateElectroshock()
    {
        _canTurnOffElbow = false;
        _isActive = false;
        foreach (var item in elbows)
        {
            item.ActivateElectroshock(effectTime);
        }

        StartCoroutine(WaitForDisableTime());
    }

    IEnumerator WaitForDisableTime()
    {
        yield return new WaitForSeconds(effectTime);
        
        DeactivateAllChargeLvlFeedback();
        _currentChargeLvlIndex = 0;
    }

    IEnumerator OnWaitUntilActivateEvent()
    {
        yield return new WaitForSeconds(timeToDeactivate);
        
        ActivateElectroshock();
    }
}
