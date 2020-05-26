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
    Image _blackOverlay;

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

        CreateBlackOverlay(parent);

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

    public BasePopup BuildPopup(Transform parent, string title, string descript, string btnText1, string btnText2, PopupsID popupId = PopupsID.BasePopup, bool isPause = false)
    {
        var popup = BuildPopup(parent, title, descript, btnText1, popupId, isPause);
        if (popup == null) return null;

        if (popup is AcceptPopup)
        {
            (popup as AcceptPopup).closeButton.GetComponentInChildren<Text>().text = btnText2;
        }

        return popup;
    }

    public bool IsAnyPopupDisplayed()
    {
        return _popupStack > 0;
    }

    public void PopupDisplayed()
    {
        _popupStack++;
    }

    public void DisplayedPopupWasClosed(string p = "")
    {
        _popupStack--;
        if (_isPauseActivated)
            _isPauseActivated = false;

        if (_blackOverlay != null)
            Destroy(_blackOverlay.gameObject);
    }

    float GetRandomAnimation()
    {
        return Random.Range(0f, 3.9f);
    }

    void CreateBlackOverlay(Transform parent)
    {
        var blackOverlay = new GameObject("blackOverlay");
        blackOverlay.transform.parent = parent;
        _blackOverlay = blackOverlay.AddComponent<Image>();
        _blackOverlay.color = new Color(0, 0, 0, 0.8f);
        _blackOverlay.raycastTarget = true;

        _blackOverlay.rectTransform.anchorMin = new Vector2(0, 0);
        _blackOverlay.rectTransform.anchorMax = new Vector2(1, 1);
        _blackOverlay.rectTransform.offsetMin = new Vector2(0, 0);
        _blackOverlay.rectTransform.offsetMax = new Vector2(0, 0);

        _blackOverlay.rectTransform.rotation = new Quaternion(0, 0, 0, 0);
        _blackOverlay.rectTransform.localScale = new Vector3(1, 1, 1);
        _blackOverlay.rectTransform.anchoredPosition3D = new Vector3(0, 0, 0);

        var canvas = parent.GetComponent<Canvas>();
        if(canvas != null)//in this mode the overlay looks very ugly
        {
            _blackOverlay.rectTransform.anchorMin = new Vector2(0, 0);
            _blackOverlay.rectTransform.anchorMax = new Vector2(1, 1);
            _blackOverlay.rectTransform.offsetMin = new Vector2(0, 0);
            _blackOverlay.rectTransform.offsetMax = new Vector2(0, 0);
        }
    }
}
