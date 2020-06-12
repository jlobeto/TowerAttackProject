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
    public GameObject settingsButtons;
    public Text devToolsText;

    GameManager _gameManager;


    private void Awake()
    {
        loadingScreen.transform.parent.gameObject.SetActive(false);
        settingsButtons.SetActive(false);
    }

    void Start ()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
        {
            SceneManager.LoadScene("GameLoadingScreen");
            return;
        }

        devToolsText.text = "devtools=" + (_gameManager.showDevTools ? "ON" : "OFF");
    }
	

	void Update ()
    {
		
	}

    public void GoToMainMap()
    {
        loadingScreen.ActivateLoadingAsyncProcess("World Selector Screen");
        menuCanvas.enabled = false;
    }

    public void SettingsButtonPressed()
    {
        settingsButtons.SetActive(true);
        mainMenuButtons.SetActive(false);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void SetDevTools()
    {
        _gameManager.showDevTools = !_gameManager.showDevTools;
        devToolsText.text = "devtool=" + (_gameManager.showDevTools ? "ON" : "OFF");
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
        settingsButtons.SetActive(false);
        mainMenuButtons.SetActive(true);
    }
}
