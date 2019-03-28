using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildWindow : EditorWindow
{
    string _buildName;
    SceneLevelListSO _levelsScenes;
    string _buildPath;
    bool _targetWindows  = true;
    bool _targetDev = true;

    [MenuItem("BUILD/Create Build")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        BuildWindow window = (BuildWindow)EditorWindow.GetWindow(typeof(BuildWindow));
        window.Show();
    }

    void OnGUI()
    {
        
        
        _buildName = EditorGUILayout.TextField("Build name", _buildName);
        GUILayout.Space(1);
        _levelsScenes = (SceneLevelListSO)EditorGUILayout.ObjectField("Level's scenes", (SceneLevelListSO)_levelsScenes, typeof(SceneLevelListSO), false);
        GUILayout.Space(1);
        if (GUILayout.Button("Save at"))
        {
            _buildPath = EditorUtility.OpenFolderPanel("Save build at", "", "");
        }
        GUILayout.Space(1);
        EditorGUILayout.LabelField("Path to save: " + _buildPath);
        GUILayout.Space(1);
        _targetWindows = EditorGUILayout.Toggle("Target Windows ", _targetWindows);
        GUILayout.Space(1);
        _targetDev = EditorGUILayout.Toggle("Target dev build", _targetDev);
        GUILayout.Space(10);

        if (GUILayout.Button("BUILD !"))
        {
            Build();
        }
    }

    void Build()
    {
        if (_levelsScenes == null)
            return;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        var list = new List<string>();
        list.Add("Assets/Scenes/MainMap.unity");

        foreach (var item in _levelsScenes.levelsScene)
        {
            list.Add("Assets/Scenes/Levels/"+item+".unity");
        }

        buildPlayerOptions.locationPathName = _buildPath + "/" + _buildName;
        buildPlayerOptions.target = !_targetWindows ? BuildTarget.StandaloneOSX : BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = _targetDev ? BuildOptions.Development : BuildOptions.None;

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        report.ToString();
        Debug.Log(report);
    }
}
