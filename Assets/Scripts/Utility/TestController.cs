using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestController : MonoBehaviour
{
    GUIStyle _guiStyle = new GUIStyle(); 
    // Use this for initialization
    void Awake ()
    {
        if (!Debug.isDebugBuild)
            Destroy(gameObject);

        _guiStyle.fontSize = 12;
        
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space(250);
        GUILayout.Label("Q: Quit To MainMap", _guiStyle);
        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(250);
        GUILayout.Label("P: Add Points", _guiStyle);
        GUILayout.BeginHorizontal();
        GUILayout.EndVertical();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FindObjectOfType<GameManager>().SetCurrentLevelInfo(null);
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<Level>().UpdatePoints(10000);
        }
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            var _lvlCanvasManager = FindObjectOfType<LevelCanvasManager>();
            var popupMan = FindObjectOfType<PopupManager>();
            popupMan.SpawnBasePopup(_lvlCanvasManager.transform);
        }*/
    }
}
