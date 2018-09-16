using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{

    public List<BasePopup> popupPrefabs = new List<BasePopup>();

    
	void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	
	void Update () {
		
	}

    public void BuildOneButtonPopup(Transform parent, string title, string descript, string btnText)
    {
        var popup = Instantiate<BasePopup>(popupPrefabs.FirstOrDefault(i => i.popupId == "OneButton"), parent);
        popup.title.text = title;
        popup.description.text = descript;
        popup.GetComponent<Animator>().SetFloat("EntryAnim", GetRandomAnimation());

        popup.GetComponentsInChildren<Button>()
            .FirstOrDefault(i => i.tag == "PopupOneButton")
            .GetComponentInChildren<Text>().text = btnText;
    }



    float GetRandomAnimation()
    {
        return Random.Range(0f, 3.9f);
    }
}
