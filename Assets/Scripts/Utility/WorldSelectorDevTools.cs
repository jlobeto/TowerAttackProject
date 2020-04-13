using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectorDevTools : MonoBehaviour
{
    public Button showToolsButton;
    public Button saveButton;
    public Button deleteProgressButton;
    public Button unlockLevelsButton;
    public Button addCoins;

    bool _isSaving = SaveSystem.canSave;
    bool _showTools;
    GameManager _gameManager;
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        if (!_gameManager.showDevTools)
        {
            Destroy(gameObject);
            return;
        }
            

        showToolsButton.GetComponentInChildren<Text>().text = (_showTools ? "Hide " : "Show ") + "Tools";
        saveButton.GetComponentInChildren<Text>().text = "Save: " + _isSaving;

    }


    void Update()
    {
        
    }

    public void ShowTools()
    {
        _showTools = !_showTools;

        deleteProgressButton.gameObject.SetActive(_showTools);
        saveButton.gameObject.SetActive(_showTools);
        unlockLevelsButton.gameObject.SetActive(_showTools);
        addCoins.gameObject.SetActive(_showTools);

        showToolsButton.GetComponentInChildren<Text>().text = (_showTools ? "Hide " : "Show ") + "Tools";
    }

    public void SetSave()
    {
        _isSaving = !_isSaving;
        SaveSystem.canSave = _isSaving;
        saveButton.GetComponentInChildren<Text>().text = "Save: " + _isSaving;
    }

    public void DeleteSaveData(bool fromEditorWindow = false)
    {
        SaveSystem.canSave = true;//for deletion i need to force the save, so it saves the empty json.
        SaveSystem.DeleteFile(SaveSystem.MINIONS_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.LEVEL_PROGRESS_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.SQUAD_ORDER_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.CURRENCY_SAVE_NAME);
        SaveSystem.DeleteFile(SaveSystem.TUTORIAL_SAVE_NAME);

        if (fromEditorWindow) return;

        var canvas = FindObjectOfType<MainMapCanvasManager>();
        var popup = _gameManager.popupManager.BuildPopup(canvas.transform, "QUIT GAME", "Exit game for properly deletion.", "Close", PopupsID.BasePopup);
        popup.AddFunction(BasePopup.FunctionTypes.ok, (string p) => { Application.Quit(); });
    }

    public void OnAddCoins()
    {
        _gameManager.User.Currency += 1000;
    }

}
