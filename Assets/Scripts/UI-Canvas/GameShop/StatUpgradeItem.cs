using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This will contain all the info that have to be shown to the user about the stat upgrade.
/// </summary>
public class StatUpgradeItem : MonoBehaviour
{
    public Button buyButton;
    public Text statName;
    public Text currValue;
    public Text nextValue;
    public Image levelBar;
    public Image nextLevelBar;
    public MinionBoughtDef.StatNames id;
    public Action<MinionBoughtDef.StatNames, StatUpgradeItem> OnBuyClick = delegate { };

    Image _buyButtonImage;
    MinionsStatsCurrencyDef _statsCurrencyDef = new MinionsStatsCurrencyDef();
    bool _noCoinsAnim;
    Color _originalColor;
    Color _targetColor;
    float _time = 0.5f;
    float _timeAux;
    bool _toRed;
    bool _toLeft;

    void Start()
    {
        _buyButtonImage = buyButton.GetComponent<Image>();
        _timeAux = _time;
        _originalColor = _buyButtonImage.color;
        _targetColor = Color.red;
    }
    
    void Update()
    {
        if (!_noCoinsAnim) return;

        var target = _toRed ? _targetColor : _originalColor;

        _buyButtonImage.color = Color.Lerp(_buyButtonImage.color, target, Time.deltaTime * 8);

        _timeAux -= Time.deltaTime;
        if (_timeAux < 0)
        {
            if (!_toRed)
                _noCoinsAnim = false;

            _timeAux = _time;
            _toRed = false;
        }
    }

    public void SetItem(int statLvl, float currValue, float nextValue, MinionBoughtDef.StatNames id, int price)
    {
        this.id = id;
        statName.text = id.ToString() + " :";
        this.currValue.text = currValue.ToString();
        this.nextValue.text = nextValue.ToString();

        levelBar.fillAmount = float.Parse(statLvl.ToString()) / ShopManager.MAX_MINION_LEVEL;

        buyButton.onClick.AddListener(() => BuyPressed());
        buyButton.GetComponentInChildren<Text>().text = price + " CHIPS";
    }

    public void NoCoinsAnimation()
    {
        if (_noCoinsAnim) return;

        _noCoinsAnim = true;
        _timeAux = _time;
        _toRed = true;
    }

    void BuyPressed()
    {
        OnBuyClick(id, this);
    }

    bool IsDove()
    {
        return _statsCurrencyDef.type == MinionType.Dove.ToString();
    }
}
