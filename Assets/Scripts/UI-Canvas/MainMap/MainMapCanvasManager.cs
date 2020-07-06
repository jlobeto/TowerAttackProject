using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMapCanvasManager : MonoBehaviour
{
    private const string STARS_TEXT = "<color=cyan>STARS:</color>";
    private const string COINS_TEXT = "<color=yellow>CHIPS:</color>";

    public Button levelNodeButton;
    public MainMap mainMap;
    public RectTransform worldsTransform;
    public WorldUI world_0_container;
    public Sprite unselectedScreenUISprite;
    public Sprite selectedScreenUISprite;
    public Text coinsText, starsText;
    [HideInInspector]
    public List<WorldUI> worldsCreated = new List<WorldUI>();

    Canvas _canvas;
    WorldUI _currentBuildingWorld;
    WorldScreenSwapSystem _swapSystem;
    SettingsPopup _settingsPopup;
    
    

    
    bool _forceUnlockAll;
    int _lvlBtn_lastWorldId;//to know when it changes de worldId
    int _lvlBtn_index;
    
    int _amountOfWorlds = 1;



    void Awake ()
    {
        _canvas = GetComponent<Canvas>();
        _swapSystem = GetComponent<WorldScreenSwapSystem>();
        _swapSystem.Init(this);

        _currentBuildingWorld = world_0_container;
        
        worldsCreated.Add(world_0_container);
    }

    private void Start()
    {
        SceneManager.sceneUnloaded += SceneUnloaded;

        starsText.text =  STARS_TEXT + mainMap.GetGameManager().User.LevelProgressManager.GetStarsAccumulated();
        coinsText.text = COINS_TEXT + mainMap.GetGameManager().User.Currency;
        mainMap.GetGameManager().User.OnCurrencyChanged += CurrencyChangedHandler;

        _settingsPopup = FindObjectOfType<SettingsPopup>();
    }

    void Update ()
    {
        

    }

    

    public void AddLevelButton(LevelInfo lvlInfo, Action<LevelInfo> onClick, GameManager gm, bool worldUnlocked, int neededToUnlock)
    {
        if(lvlInfo.worldId != _lvlBtn_lastWorldId)
        {
            var newWorldUI = Instantiate<WorldUI>(world_0_container, worldsTransform);
            newWorldUI.transform.position += Vector3.right * _canvas.pixelRect.width * lvlInfo.worldId;
            newWorldUI.name = "world_" + lvlInfo.worldId;
            worldsCreated.Add(newWorldUI);
            newWorldUI.Init(lvlInfo.worldId, worldUnlocked, neededToUnlock);
            newWorldUI.gameObject.SetActive(false);
            _currentBuildingWorld = newWorldUI;

            _amountOfWorlds++;
            _swapSystem.amountOfWorlds = _amountOfWorlds;

            _lvlBtn_index = 0;
        }

        if (lvlInfo.id == -1)
        {
            var currentLevelProgress = mainMap.GetGameManager().User.LevelProgressManager.GetProgress(lvlInfo.id);
            if (currentLevelProgress != null && currentLevelProgress.won)
                return;//don't want to display level-1 if it's already done
        }

        var btn = Instantiate<Button>(levelNodeButton, _currentBuildingWorld.grid.transform);
        LevelInfo lazyLvlInfo = new LevelInfo();
        lazyLvlInfo = lvlInfo;
        btn.onClick.AddListener(() => onClick(lazyLvlInfo));
        btn.name = "levelNodeButton_" + _lvlBtn_index;   

        var node = btn.GetComponent<LevelNode> ();
		node.Init (lazyLvlInfo , gm, btn);

        _lvlBtn_lastWorldId = lvlInfo.worldId;
        _lvlBtn_index++;
    }
    
    public void ShowWorldAtSceneInit(int id)
    {
        //if (id == 0) return;

        _swapSystem.ShowWorldAtSceneInit(id);
    }


    void CurrencyChangedHandler(int currency)
    {
        coinsText.text = COINS_TEXT + currency;
    }

    void SceneUnloaded(Scene scene)
    {
        mainMap.GetGameManager().User.OnCurrencyChanged -= CurrencyChangedHandler;
    }


    public void DisplaySettingsPopup()
    {
        _settingsPopup.DisplayPopup();
    }






    /// <summary>
    /// USed by the dev button
    /// </summary>
    public void ForceUnlockAllLevels()
    {
        foreach (var item in worldsCreated)
        {
            if(item.GetInstanceID() != world_0_container.GetInstanceID())
                Destroy(item.gameObject);
        }

        worldsCreated = new List<WorldUI>();
        worldsCreated.Add(world_0_container);

        _lvlBtn_lastWorldId = 0;
        _currentBuildingWorld = world_0_container;
        world_0_container.DeleteGridChildren();
        

        _amountOfWorlds = 1;
        mainMap.GetGameManager().User.LevelProgressManager.ForceWinAllLevels();
        mainMap.CreateLevelNodes();

        //_worldsCreated[_currentWorldOnScreen].gameObject.SetActive(true);
        
    }
}

