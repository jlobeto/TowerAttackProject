using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


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
        SceneManager.LoadScene("World Selector Screen");
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
