using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapCanvasManager : MonoBehaviour
{
    public Button levelNodeButton;
    public MainMap mainMap;
    public RectTransform worldsBtnContainer;
    public RectTransform worldsNameContainer;
    public RectTransform worldsArrowsContainer;
    public Text worldOne_NameText;
    public Image toLeftArrow;
    public Image toRightArrow;

    Canvas _canvas;
    GridLayoutGroup _currentBuilding;
    GridLayoutGroup _levelNodesContainer;
    
    List<GridLayoutGroup> _gridLayouts = new List<GridLayoutGroup>();
    Vector3 _toPosition;//for grids movements;
    Vector3 _fromPosition;//for grids movements;
    bool _isMovingGrid;
    bool _forceUnlockAll;
    int _lvlBtn_lastWorldId;//to know when it changes de worldId
    float _mouseOnPressXPos;
    float _mouseOnReleaseXPos;
    int _currentWorldOnScreen;


    void Awake ()
    {
        _canvas = GetComponent<Canvas>();
        _levelNodesContainer = GetComponentInChildren<GridLayoutGroup>();
        _currentBuilding = _levelNodesContainer;
        _gridLayouts.Add(_levelNodesContainer);
        _toPosition = _currentBuilding.transform.position;

        toLeftArrow.enabled = false;
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
            if (_currentWorldOnScreen == _gridLayouts.Count - 1) return;

            _toPosition = worldsBtnContainer.position - Vector3.right * _canvas.pixelRect.width;
            _currentWorldOnScreen++;
        }
        else //left to right
        {
            if (_currentWorldOnScreen == 0) return;

            _toPosition = worldsBtnContainer.position + Vector3.right * _canvas.pixelRect.width;
            _currentWorldOnScreen--;
        }

        _isMovingGrid = true;
    }

    void GridMovement()
    {
        if (!_isMovingGrid) return;

        worldsBtnContainer.position = Vector3.Lerp(worldsBtnContainer.position, _toPosition, Time.deltaTime * 10);
        worldsNameContainer.position = Vector3.Lerp(worldsNameContainer.position, _toPosition, Time.deltaTime * 10);

        if(Mathf.Abs(Vector3.Distance(worldsBtnContainer.position, _toPosition)) < 1f)
        {
            worldsBtnContainer.position = worldsNameContainer.position = _toPosition;
            if (_currentWorldOnScreen == 0)
                toLeftArrow.enabled = false;
            else if(_currentWorldOnScreen == _gridLayouts.Count-1)
                toRightArrow.enabled = false;
            else
                toRightArrow.enabled = toLeftArrow.enabled = true;

            _isMovingGrid = false;
        }
    }

    public void AddLevelButton(LevelInfo lvlInfo, Action<LevelInfo> onClick, GameManager gm)
    {
        if(lvlInfo.worldId != _lvlBtn_lastWorldId)
        {
            var grid = Instantiate<GridLayoutGroup>(_levelNodesContainer, worldsBtnContainer);
            DeleteAllChildren(grid);
            _currentBuilding = grid;
            _gridLayouts.Add(grid);
            grid.transform.position += Vector3.right * _canvas.pixelRect.width * lvlInfo.worldId;
            grid.name = "world_" + lvlInfo.worldId;

            var worldName = Instantiate<Text>(worldOne_NameText, worldsNameContainer);
            worldName.transform.position += Vector3.right * _canvas.pixelRect.width * lvlInfo.worldId;
            worldName.text = "WORLD " + (lvlInfo.worldId + 1);
            worldName.name = "world_"+ lvlInfo.worldId + "_text";
            
        }

        var btn = Instantiate<Button>(levelNodeButton, _currentBuilding.transform);
        LevelInfo lazyLvlInfo = new LevelInfo();
        lazyLvlInfo = lvlInfo;
        btn.onClick.AddListener(() => onClick(lazyLvlInfo));
		var node = btn.GetComponent<LevelNode> ();
		node.Init (lazyLvlInfo , gm, btn);

        _lvlBtn_lastWorldId = lvlInfo.worldId;
    }

    /// <summary>
    /// USed by the dev button at the upper left corner.
    /// </summary>
    public void ForceUnlockAllLevels()
    {
        for (int i = 0; i < _gridLayouts.Count; i++)
        {
            if (i == 0) continue;

            Destroy(_gridLayouts[i].gameObject);
        }
        _gridLayouts = new List<GridLayoutGroup>();
        //_gridLayouts.Add(_levelNodesContainer);

        mainMap.GetRealGameManager().User.LevelProgressManager.ForceWinAllLevels();
        mainMap.CreateLevelNodes();
    }

    void DeleteAllChildren(GridLayoutGroup grid)
    {
        foreach (Transform item in grid.transform)
        {
            Destroy(item.gameObject);
        }
    }
    
}

