using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OneBtnPopup : BasePopup
{

    public Button actionButton;
	
	protected override void Start () {
        /*title.text = "Game Over !";
        description.text = "Try Again";*/
    }
	
	
	void Update () {
		
	}

    public override void OnCloseButton()
    {
        OnActionButton();
    }

    public virtual void OnActionButton()
    {
        FindObjectOfType<GameManager>().SetCurrentLevelInfo(null);
        SceneManager.LoadScene(0);
    }
}
