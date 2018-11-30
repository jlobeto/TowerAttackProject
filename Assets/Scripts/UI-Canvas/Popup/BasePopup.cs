using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePopup : MonoBehaviour
{
    public PopupsID popupId;
    //public Button closeButton;
    //these texts will be loaded from JSON, using popupId;
    public Text title;
    public Text description;
    public Button actionButton;


    protected virtual void Start ()
    {
        //title.text = "Title Text";
        //description.text = "Description Text";
    }
	
	
	void Update () {
		
	}

    public virtual void OnCloseButton()
    {
        Destroy(gameObject);
    }
}
