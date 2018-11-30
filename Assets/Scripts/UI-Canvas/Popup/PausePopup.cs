using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePopup : BasePopup
{

	public override void OnCloseButton()
    {
		Time.timeScale = 1;//por si esta en el popup de pausa
		Destroy (gameObject);
    }

    public void OnAction()
    {
        Time.timeScale = 1;//por si esta en el popup de pausa
        FindObjectOfType<GameManager>().SetCurrentLevelInfo(null);
        SceneManager.LoadScene(0);
    }
}
