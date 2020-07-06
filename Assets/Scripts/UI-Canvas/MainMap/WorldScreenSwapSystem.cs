using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldScreenSwapSystem : MonoBehaviour
{
    public Slider worldSelectorSlider;
    public Image handlerImageSlider;
    public Text handlerImageSliderText;
    public Color handlerColorFilled;
    public Color handlerColorNotFilled;
    public bool blockSwapWorldMovement;
    public int amountOfWorlds;

    PopupManager _popupManager;
    Canvas _canvas;
    MainMapCanvasManager _mainMapCanvas;
    List<Image> _worldPointHandlerHolders;//list of "world points" images in the slider
    Vector3 _toPosition;//for grids movements;
    Vector3 _toLocksPosition;//for padlocks (UI) movements;
    Vector3 _toNamesPosition;//for title movements;
    Vector2 _sliderHandlerInitSize;
    bool _isMovingGrid;
    bool _usingSlinder;
    float _mouseOnPressXPos;
    float _mouseOnReleaseXPos;
    float _lastSliderValue;

    int _currentWorldOnScreen;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _popupManager = FindObjectOfType<PopupManager>();
    }
    
    void Update()
    {
        if (_usingSlinder) return;

        if (Input.GetMouseButtonDown(0))
        {
            _mouseOnPressXPos = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _mouseOnReleaseXPos = Input.mousePosition.x;
            if (!_popupManager.IsAnyPopupDisplayed() && !_usingSlinder)
                CheckMovement();
        }

        GridMovement();
    }

    public void Init(MainMapCanvasManager mainmap)
    {
        _mainMapCanvas = mainmap;
    }
    
    public void ShowWorldAtSceneInit(int id)
    {
        if (_mainMapCanvas.worldsCreated.Count > id)
        {
            _currentWorldOnScreen = id;
            _mainMapCanvas.worldsCreated[_currentWorldOnScreen].gameObject.SetActive(true);
            HideNonSelectedWorlds();
        }
        SetSliderAtInit();
    }

    void SetSliderAtInit()
    {
        worldSelectorSlider.maxValue = _mainMapCanvas.worldsCreated.Count - 1;

        _worldPointHandlerHolders = new List<Image>();
        foreach (var item in _mainMapCanvas.worldsCreated)
        {
            var img = Instantiate(handlerImageSlider, handlerImageSlider.transform.parent);
            Destroy(img.GetComponentInChildren<Text>());
            _worldPointHandlerHolders.Add(img);
        }

        handlerImageSlider.rectTransform.SetAsLastSibling();
        _sliderHandlerInitSize = handlerImageSlider.transform.localScale;
        var pointerCallbacks = worldSelectorSlider.gameObject.AddComponent<OnCustomPointerCallback>();
        pointerCallbacks.AddListener(OnCustomPointerCallback.Listener.pointerDown, OnPointerDown);
        pointerCallbacks.AddListener(OnCustomPointerCallback.Listener.pointerUp, OnPointerUp);

        var anchoredDiff = (float)1 / (_mainMapCanvas.worldsCreated.Count - 1);
        var currentPos = 0f;

        foreach (var img in _worldPointHandlerHolders)
        {
            img.rectTransform.anchorMin = new Vector2(currentPos, 0);
            img.rectTransform.anchorMax = new Vector2(currentPos, 1);
            currentPos += anchoredDiff;
        }

        worldSelectorSlider.value = _currentWorldOnScreen;
    }

    #region Normal Swap Movement
    /// <summary>
    /// Always do it on the release funtion so we have onpress and onrelease.
    /// </summary>
    void CheckMovement()
    {
        if (_isMovingGrid) return;

        var max = Mathf.Max(_mouseOnPressXPos, _mouseOnReleaseXPos);
        var min = Mathf.Min(_mouseOnPressXPos, _mouseOnReleaseXPos);
        var delta = max - min;

        if (delta < 70)
            return;

        //right to left
        if (_mouseOnPressXPos > _mouseOnReleaseXPos)
        {
            if (_currentWorldOnScreen == amountOfWorlds - 1) return;

            _toPosition = _mainMapCanvas.worldsTransform.position - Vector3.right * GetCanvasWidth();
            _currentWorldOnScreen++;
        }
        else //left to right
        {
            if (_currentWorldOnScreen == 0) return;

            _toPosition = _mainMapCanvas.worldsTransform.position + Vector3.right * GetCanvasWidth();
            _currentWorldOnScreen--;
        }

        _mainMapCanvas.worldsCreated[_currentWorldOnScreen].gameObject.SetActive(true);

        worldSelectorSlider.value = _currentWorldOnScreen;
        _isMovingGrid = true;
    }

    void GridMovement()
    {
        if (!_isMovingGrid) return;

        _mainMapCanvas.worldsTransform.position = Vector3.Lerp(_mainMapCanvas.worldsTransform.position, _toPosition, Time.deltaTime * 10);

        if (Mathf.Abs(Vector3.Distance(_mainMapCanvas.worldsTransform.position, _toPosition)) < 1f)
        {
            _mainMapCanvas.worldsTransform.position = _toPosition;
            HideNonSelectedWorlds();

            _isMovingGrid = false;
        }
    }
    #endregion

    void HideNonSelectedWorlds()
    {
        int i = 0;
        foreach (var item in _mainMapCanvas.worldsCreated)
        {
            if (_currentWorldOnScreen != i && item.gameObject.activeSelf)
                item.gameObject.SetActive(false);

            i++;
        }
    }

    public void SliderOnValueChanged()
    {
        var currentAnchoredPos = handlerImageSlider.rectTransform.anchorMin.x;
        foreach (var item in _worldPointHandlerHolders)
        {
            var itemAnchorPos = item.rectTransform.anchorMin.x;
            if (itemAnchorPos <= currentAnchoredPos)
                if (item.color != handlerColorFilled)
                    item.color = handlerColorFilled;

            if (itemAnchorPos > currentAnchoredPos)
                if (item.color != handlerColorNotFilled)
                    item.color = handlerColorNotFilled;
        }

        _currentWorldOnScreen = (int)worldSelectorSlider.value;

        var diff = (_lastSliderValue - worldSelectorSlider.value);
        var newPosition = _mainMapCanvas.worldsTransform.position + Vector3.right * GetCanvasWidth() * diff;
        _mainMapCanvas.worldsTransform.position = newPosition;

        if (worldSelectorSlider.value < _lastSliderValue)
        {

        }
        else if (worldSelectorSlider.value > _lastSliderValue)
        {

        }

        _mainMapCanvas.worldsCreated[_currentWorldOnScreen].gameObject.SetActive(true);
        HideNonSelectedWorlds();



        handlerImageSliderText.text = (worldSelectorSlider.value+1).ToString();
        _lastSliderValue = worldSelectorSlider.value;
    }

    void OnPointerDown()
    {
        handlerImageSlider.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        _usingSlinder = true;
    }
    void OnPointerUp()
    {
        handlerImageSlider.transform.localScale = _sliderHandlerInitSize;
        StartCoroutine(WaitToNextFrameToFreeSwapMove());
    }

    IEnumerator WaitToNextFrameToFreeSwapMove()
    {
        yield return new WaitForEndOfFrame();
        _usingSlinder = false;
    }

    float GetCanvasWidth()
    {
        return _canvas.pixelRect.width;
    }
}
