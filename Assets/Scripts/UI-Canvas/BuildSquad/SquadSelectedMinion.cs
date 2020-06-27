using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadSelectedMinion : MonoBehaviour
{
    public Button button;
    public Image minionPic;
    public Text text;
    public Action<MinionType> onMinionClick = delegate { };
    public MinionType minionType = MinionType.Runner;

    public bool IsEmpty { get { return _isEmpty; } }

    bool _isEmpty = true;
    Color _originalColor;
    Color _targetColor;
    bool _doingRedColorAnim;
    bool _toRed;
    float _time = 0.25f;
    float _timeAux;

    void Awake()
    {
        minionPic.enabled = false;
        text.text = "";

        _originalColor = button.colors.normalColor;
        _targetColor = Color.red;
    }

    
    void Update()
    {
        if (!_doingRedColorAnim) return;

        var target = _toRed ? _targetColor : _originalColor;

        var colorBlock = button.colors;
        colorBlock.normalColor = Color.Lerp(colorBlock.normalColor, target, Time.deltaTime * 16);
        button.colors = colorBlock;

        _timeAux -= Time.deltaTime;
        if (_timeAux < 0)
        {
            if (!_toRed)
                _doingRedColorAnim = false;

            _timeAux = _time;
            _toRed = false;
        }
    }

    public void SetMinion(MinionType type, GameManager gm)
    {
        text.text = type.ToString();

        minionPic.enabled = true;
        minionPic.sprite = gm.LoadedAssets.GetSpriteByName(text.text);

        _isEmpty = false;
        minionType = type;
    }

    public void ResetMinion()
    {
        minionPic.enabled = false;
        _isEmpty = true;
        text.text = "";
        minionType = MinionType.Runner;
    }

    public void OnClickSelectedMinion()
    {
        onMinionClick(minionType);
    }

    public void DoRedColorAnimation()
    {
        if (_doingRedColorAnim) return;

        _doingRedColorAnim = true;
        _toRed = true;
        _timeAux = _time;
    }

}
