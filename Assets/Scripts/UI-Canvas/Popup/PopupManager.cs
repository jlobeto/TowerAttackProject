using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public List<BasePopup> popupPrefabs = new List<BasePopup>();

    int _popupStack = 0;
    bool _isPauseActivated;

    void Start()
    {
        DontDestroyOnLoad(this);
    }


    void Update() {

    }

    public BasePopup BuildPopup(Transform parent, string title, string descript, string btnText, PopupsID popupId = PopupsID.BasePopup, bool isPause = false)
    {
        if (popupPrefabs.All(i => i.popupId != popupId))
            return null;

        if (isPause && _isPauseActivated)
            return null;
        else if (isPause)
            _isPauseActivated = true;

        var popup = Instantiate<BasePopup>(popupPrefabs.FirstOrDefault(i => i.popupId == popupId), parent);
        popup.title.text = title.ToUpper();
        popup.description.text = descript.ToUpper();
        //popup.GetComponent<Animator>().SetFloat("EntryAnim", GetRandomAnimation());
        popup.isShowing = true;
        if (popup.okButton != null)
            popup.okButton.GetComponentInChildren<Text>().text = btnText.ToUpper();

        popup.AddFunction(BasePopup.FunctionTypes.close, DisplayedPopupWasClosed);
        _popupStack++;

        return popup;
    }

    public bool IsAnyPopupDisplayed()
    {
        return _popupStack > 0;
    }

    public void ShopPopupDisplayed()
    {
        _popupStack++;
    }

    public void DisplayedPopupWasClosed()
    {
        _popupStack--;

        if(_isPauseActivated)
            _isPauseActivated = false;
    }

    float GetRandomAnimation()
    {
        return Random.Range(0f, 3.9f);
    }
}
