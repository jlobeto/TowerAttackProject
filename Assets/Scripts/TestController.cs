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
        GUILayout.Label("R: reset scene", _guiStyle);
        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(250);
        GUILayout.Label("P: Add Points", _guiStyle);
        GUILayout.BeginHorizontal();
        GUILayout.EndVertical();
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Sandbox Scene");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            FindObjectOfType<Level>().UpdatePoints(10000);
        }
    }
}
