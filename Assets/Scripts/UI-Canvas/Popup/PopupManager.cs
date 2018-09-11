using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopupManager : MonoBehaviour
{

    public List<BasePopup> popupPrefabs = new List<BasePopup>();

    
	void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	
	void Update () {
		
	}

    public void BuildEndLevelPopup(Transform parent, string title, string descript)
    {
        var popup = Instantiate<BasePopup>(popupPrefabs.FirstOrDefault(i => i.popupId == "RetryLevel"), parent);
        popup.title.text = title;
        popup.description.text = descript;
        popup.GetComponent<Animator>().SetFloat("EntryAnim", GetRandomAnimation());
    }



    float GetRandomAnimation()
    {
        return Random.Range(0f, 3.9f);
    }
}
