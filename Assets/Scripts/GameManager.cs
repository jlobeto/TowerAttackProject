using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CurrentScene {
        MainMap,
        Gameplay
    }

    /// <summary>
    /// This is set when a level node is clicked, When returning from a level, this is set to null.
    /// Using this to set some variables to the Level Object on Level Scene
    /// </summary>
    public LevelInfo currentLevelInfo;
    public CurrentScene currentScene = CurrentScene.MainMap;

	void Awake() {
        DontDestroyOnLoad(this);
	}
	
	
	void Update () {
		
	}
}
