using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : BasePopup
{
    public Slider musicSlider;
    public Slider soundSlider;
    public Text devToolsText;

    GameManager _gameManager;
    SoundManager _soundManager;
    PopupManager _popupManager;
    Canvas _canvas;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
        base.Awake();
        
        _canvas = GetComponent<Canvas>();

        
    }

    protected override void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _gameManager = FindObjectOfType<GameManager>();
        _popupManager = FindObjectOfType<PopupManager>();

        devToolsText.text = "devtools=" + (_gameManager.showDevTools ? "ON" : "OFF");
    }


    void Update()
    {

    }


    public override void DisplayPopup()
    {
        _canvas.enabled = true;
        _popupManager.PopupDisplayed();
        Time.timeScale = 0;
        base.DisplayPopup();
    }

    public void SetDevTools()
    {
        _gameManager.showDevTools = !_gameManager.showDevTools;
        devToolsText.text = "devtool=" + (_gameManager.showDevTools ? "ON" : "OFF");
        _soundManager.PlaySound(SoundFxNames.button_pressed);
    }

    protected override void OnClosePopup()
    {
        _soundManager.PlaySound(SoundFxNames.back_button);
        Time.timeScale = 1;
        _canvas.enabled = false;
        _popupManager.DisplayedPopupWasClosed();
        ExecuteFunctions(FunctionTypes.close);
    }

    public void MusicVolumeChangeHandler()
    {
        _soundManager.AdjustMusicVol(musicSlider.value);
    }

    public void SoundFXVolumeChangeHandler()
    {
        _soundManager.AdjustSoundFXVol(soundSlider.value);
    }
    
}
