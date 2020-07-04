using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameLoadingScreen loadingScreen;
    public Canvas menuCanvas;
    public GameObject mainMenuButtons;

    GameManager _gameManager;
    SettingsPopup _settingsPopup;


    private void Awake()
    {
        loadingScreen.transform.parent.gameObject.SetActive(false);
        _settingsPopup = FindObjectOfType<SettingsPopup>();
    }

    void Start ()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            SceneManager.LoadScene("GameLoadingScreen");
            return;
        }
    }
	

	void Update ()
    {
		
	}

    public void GoToMainMap()
    {
        SoundManager.instance.PlaySound(SoundFxNames.button_pressed);
        loadingScreen.ActivateLoadingAsyncProcess("World Selector Screen");
        menuCanvas.enabled = false;
    }

    public void SettingsButtonPressed()
    {

        SoundManager.instance.PlaySound(SoundFxNames.button_pressed);
        _settingsPopup.DisplayPopup();
    }

    public void ExitApp()
    {
        SoundManager.instance.PlaySound(SoundFxNames.back_button);
        Application.Quit();
    }

    
    
    public void DeleteProgress()
    {
        SaveSystem.canSave = true;//for deletion i need to force the save, so it saves the empty json.
        SaveSystem.DeleteFile(SaveSystem.MINIONS_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.LEVEL_PROGRESS_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.SQUAD_ORDER_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.CURRENCY_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.TUTORIAL_SAVE_NAME);
        

        var canvas = FindObjectOfType<MainMapCanvasManager>();
        var popup = _gameManager.popupManager.BuildPopup(menuCanvas.transform, "QUIT GAME", "Exit game for properly deletion.", "Close", PopupsID.BasePopup);
        popup.AddFunction(BasePopup.FunctionTypes.ok, (string p) => { Application.Quit(); });
    }

    public void GoBack()
    {
        mainMenuButtons.SetActive(true);
    }
}
