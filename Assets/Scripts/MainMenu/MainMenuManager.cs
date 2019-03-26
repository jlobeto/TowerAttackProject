using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{


	void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}

    public void GoToMainMap()
    {
        SceneManager.LoadScene("NewMainMap");
    }

    public void ExitApp()
    {
        if(Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
