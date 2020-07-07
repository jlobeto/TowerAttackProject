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
    PopupManager _popupManager;
    Canvas _canvas;
    CameraMovement _gameplayCamera;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
        base.Awake();
        
        _canvas = GetComponent<Canvas>();

        
    }

    protected override void Start()
    {
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
        if(_gameManager.CurrentScene == GameManager.GameScenes.Gameplay)
        {
            if (_gameplayCamera == null)
                _gameplayCamera = FindObjectOfType<CameraMovement>();
            _gameplayCamera.SetCameraMovement(false);
        }
        base.DisplayPopup();
    }

    public void SetDevTools()
    {
        _gameManager.showDevTools = !_gameManager.showDevTools;
        devToolsText.text = "devtool=" + (_gameManager.showDevTools ? "ON" : "OFF");
        SoundManager.instance.PlaySound(SoundFxNames.button_pressed);
    }

    protected override void OnClosePopup()
    {
        SoundManager.instance.PlaySound(SoundFxNames.back_button);
        Time.timeScale = 1;
        _canvas.enabled = false;
        _popupManager.DisplayedPopupWasClosed();

        if (_gameManager.CurrentScene == GameManager.GameScenes.Gameplay)
        {
            if (_gameplayCamera == null)
                _gameplayCamera = FindObjectOfType<CameraMovement>();
            _gameplayCamera.SetCameraMovement(true);
        }

        ExecuteFunctions(FunctionTypes.close);
    }

    public void MusicVolumeChangeHandler()
    {
        SoundManager.instance.AdjustMusicVol(musicSlider.value);
    }

    public void SoundFXVolumeChangeHandler()
    {
        SoundManager.instance.AdjustSoundFXVol(soundSlider.value);
    }
    
}
