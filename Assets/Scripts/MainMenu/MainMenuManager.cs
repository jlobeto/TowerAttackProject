using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameLoadingScreen loadingScreen;
    public Canvas menuCanvas;

    private void Awake()
    {
        loadingScreen.transform.parent.gameObject.SetActive(false);
    }

    void Start ()
    {
        var _gameManager = FindObjectOfType<GameManager>();
        if (_gameManager == null)
            SceneManager.LoadScene(0);
    }
	

	void Update ()
    {
		
	}

    public void GoToMainMap()
    {
        loadingScreen.ActivateLoadingAsyncProcess("World Selector Screen");
        menuCanvas.enabled = false;
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
