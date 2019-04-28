using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapCanvasManager : MonoBehaviour
{
    public Button levelNodeButton;
    public MainMap mainMap;
    public RectTransform worldsTransform;
    public WorldUI world_0_container;
    public Image selectedScreenUI;
    public Sprite unselectedScreenUISprite;
    public Sprite selectedScreenUISprite;

    Canvas _canvas;
    WorldUI _currentBuildingWorld;
    
    List<WorldUI> _worldsCreated = new List<WorldUI>();
    List<Image> _screenSelectorsUI = new List<Image>();

    Vector3 _toPosition;//for grids movements;
    Vector3 _toLocksPosition;//for padlocks (UI) movements;
    Vector3 _toNamesPosition;//for title movements;
    bool _isMovingGrid;
    bool _forceUnlockAll;
    int _lvlBtn_lastWorldId;//to know when it changes de worldId
    float _mouseOnPressXPos;
    float _mouseOnReleaseXPos;
    int _amountOfWorlds = 1;
    int _currentWorldOnScreen;


    void Awake ()
    {
        _canvas = GetComponent<Canvas>();
        _currentBuildingWorld = world_0_container;

        _screenSelectorsUI.Add(selectedScreenUI);
    }
	
	void Update ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _mouseOnPressXPos = Input.mousePosition.x;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            _mouseOnReleaseXPos = Input.mousePosition.x;
            if(! mainMap.GetRealGameManager().popupManager.IsAnyPopupDisplayed())
                CheckMovement();
        }

        GridMovement();
	}

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
            if (_currentWorldOnScreen == _amountOfWorlds-1) return;

            _toPosition = worldsTransform.position - Vector3.right * _canvas.pixelRect.width;
            _currentWorldOnScreen++;
        }
        else //left to right
        {
            if (_currentWorldOnScreen == 0) return;

            _toPosition = worldsTransform.position + Vector3.right * _canvas.pixelRect.width;
            _currentWorldOnScreen--;
        }

        _isMovingGrid = true;
    }

    void GridMovement()
    {
        if (!_isMovingGrid) return;

        worldsTransform.position = Vector3.Lerp(worldsTransform.position, _toPosition, Time.deltaTime * 10);

        if (Mathf.Abs(Vector3.Distance(worldsTransform.position, _toPosition)) < 1f)
        {
            worldsTransform.position = _toPosition;

            foreach (var item in _screenSelectorsUI)
                item.sprite = unselectedScreenUISprite;

            _screenSelectorsUI[_currentWorldOnScreen].sprite = selectedScreenUISprite;
            _isMovingGrid = false;
        }
    }

    public void AddLevelButton(LevelInfo lvlInfo, Action<LevelInfo> onClick, GameManager gm, bool worldUnlocked, int neededToUnlock)
    {
        if(lvlInfo.worldId != _lvlBtn_lastWorldId)
        {
            var newWorldUI = Instantiate<WorldUI>(world_0_container, worldsTransform);
            newWorldUI.transform.position += Vector3.right * _canvas.pixelRect.width * lvlInfo.worldId;
            newWorldUI.name = "world_" + lvlInfo.worldId;
            _worldsCreated.Add(newWorldUI);

            
            newWorldUI.Init(lvlInfo.worldId, worldUnlocked, neededToUnlock);

            _currentBuildingWorld = newWorldUI;

            _amountOfWorlds++;

            CreateScreenPointUI(lvlInfo.worldId);
        }

        var btn = Instantiate<Button>(levelNodeButton, _currentBuildingWorld.grid.transform);
        LevelInfo lazyLvlInfo = new LevelInfo();
        lazyLvlInfo = lvlInfo;
        btn.onClick.AddListener(() => onClick(lazyLvlInfo));
		var node = btn.GetComponent<LevelNode> ();
		node.Init (lazyLvlInfo , gm, btn);

        _lvlBtn_lastWorldId = lvlInfo.worldId;
    }

    /// <summary>
    /// Build the bottom circle UI to know how many worlds are.
    /// </summary>
    void CreateScreenPointUI(int worldId)
    {
        if (_amountOfWorlds > _screenSelectorsUI.Count)
        {
            var screenPoint = Instantiate<Image>(selectedScreenUI, selectedScreenUI.transform.parent);
            screenPoint.sprite = unselectedScreenUISprite;
            screenPoint.name = "SelectedScreen_" + worldId;
            _screenSelectorsUI.Add(screenPoint);
        }
    }

    /// <summary>
    /// USed by the dev button at the upper left corner.
    /// </summary>
    public void ForceUnlockAllLevels()
    {
        foreach (var item in _worldsCreated)
        {
            Destroy(item.gameObject);
        }

        _lvlBtn_lastWorldId = 0;
        _currentBuildingWorld = world_0_container;
        world_0_container.DeleteGridChildren();
        

        _amountOfWorlds = 1;
        mainMap.GetRealGameManager().User.LevelProgressManager.ForceWinAllLevels();
        mainMap.CreateLevelNodes();
    }


    
}

