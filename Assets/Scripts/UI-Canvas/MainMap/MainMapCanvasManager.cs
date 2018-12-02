﻿using System;
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
    public Text worldOne_NameText;

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


    void Awake ()
    {
        _canvas = GetComponent<Canvas>();
        _levelNodesContainer = GetComponentInChildren<GridLayoutGroup>();
        _currentBuilding = _levelNodesContainer;
        _gridLayouts.Add(_levelNodesContainer);
        _toPosition = _currentBuilding.transform.position;
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


        _isMovingGrid = true;

        //right to left
        if (_mouseOnPressXPos > _mouseOnReleaseXPos)
        {
            _toPosition = worldsBtnContainer.position - Vector3.right * _canvas.pixelRect.width;
            
        }
        else //left to right
        {
            _toPosition = worldsBtnContainer.position + Vector3.right * _canvas.pixelRect.width;
        }
    }

    void GridMovement()
    {
        if (!_isMovingGrid) return;

        worldsBtnContainer.position = Vector3.Lerp(worldsBtnContainer.position, _toPosition, Time.deltaTime * 10);
        worldsNameContainer.position = Vector3.Lerp(worldsNameContainer.position, _toPosition, Time.deltaTime * 10);

        if(Mathf.Abs(Vector3.Distance(worldsBtnContainer.position, _toPosition)) < 1f)
        {
            worldsBtnContainer.position = worldsNameContainer.position = _toPosition;
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
        foreach (var item in _gridLayouts)
        {
            DeleteAllChildren(item);
        }

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

