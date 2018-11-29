using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapCanvasManager : MonoBehaviour
{
    public Button levelNodeButton;
    public MainMap mainMap;

    GridLayoutGroup _levelNodesContainer;
    bool _forceUnlockAll;

	void Awake () {
        _levelNodesContainer = GetComponentInChildren<GridLayoutGroup>();
	}
	
	void Update () {
		
	}

	public void AddLevelButton(LevelInfo lvlInfo, Action<LevelInfo> onClick, GameManager gm)
    {
        var btn = Instantiate<Button>(levelNodeButton, _levelNodesContainer.transform);
        LevelInfo lazyLvlInfo = new LevelInfo();
        lazyLvlInfo = lvlInfo;
        btn.onClick.AddListener(() => onClick(lazyLvlInfo));
        btn.GetComponentInChildren<Text>().text = "Level " + lazyLvlInfo.id;
		var node = btn.GetComponent<LevelNode> ();
		node.Init (lazyLvlInfo , gm, btn);
    }

    public void ForceUnlockAllLevels()
    {
        foreach (Transform item in _levelNodesContainer.transform)
        {
            Destroy(item.gameObject);
        }

        mainMap.GetRealGameManager().User.LevelProgressManager.ForceWinAllLevels();
        mainMap.CreateLevelNodes();
    }

    public void ForceUnlockNextLevel()
    {

    }
}
