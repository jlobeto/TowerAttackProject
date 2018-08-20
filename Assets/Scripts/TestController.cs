using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
        GUILayout.Label("R: reset scene", _guiStyle);
        GUILayout.Label("Left Click: spawn runner", _guiStyle);
        GUILayout.Label("Right Click: spawn tank", _guiStyle);
        GUILayout.Label("Middle Click: spawn dove", _guiStyle);
        GUILayout.EndVertical();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Sandbox Scene");
        }
	}
}
