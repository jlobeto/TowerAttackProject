using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[ExecuteInEditMode]
public class BakeLightsWindow : EditorWindow
{
    string _pathToScenes = "Assets/Scenes/Levels";
    string[] _lightmapResolution = new string[] { "1024", "2048" };
    int _lightMapResIndex;
    bool _baking;
    string[] _realPaths;
    int _currentSceneBuildingIndex = 0;
    List<string> _scenesBuilt = new List<string>();
    Dictionary<string, bool> _sceneToggle = new Dictionary<string, bool>();
    Vector2 _scrollPosition;

    [MenuItem("UTILITIES/Bake Scenes Lights")]
    static void Init()
    {
        BakeLightsWindow window = (BakeLightsWindow)EditorWindow.GetWindow(typeof(BakeLightsWindow));
        window.Show();
    }

    void Update()
    {
        if (!_baking) return;

        if(!Lightmapping.isRunning)
        {
            _scenesBuilt.Add(_realPaths[_currentSceneBuildingIndex]); 
            EditorSceneManager.OpenScene(_realPaths[_currentSceneBuildingIndex]);

            LightmapEditorSettings.maxAtlasSize = int.Parse(_lightmapResolution[_lightMapResIndex]);
            LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveCPU;
            LightmapEditorSettings.mixedBakeMode = MixedLightingMode.Subtractive;
            LightmapEditorSettings.lightmapsMode = LightmapsMode.NonDirectional;
            LightmapEditorSettings.filteringMode = LightmapEditorSettings.FilterMode.Auto;
            Lightmapping.realtimeGI = false;

            Lightmapping.BakeAsync();
            _currentSceneBuildingIndex++;

            if (_realPaths.Length == _currentSceneBuildingIndex)
                _baking = false;
        }
        
    }

    void OnGUI()
    {

        _pathToScenes = EditorGUILayout.TextField("Path To Scenes", _pathToScenes);
        GUILayout.Space(1);
        _lightMapResIndex = EditorGUILayout.Popup("Lightmap resolution", _lightMapResIndex, _lightmapResolution);

        GUILayout.Space(1);

        var paths = new string[] { _pathToScenes };

        var p = AssetDatabase.FindAssets("t:scene", paths);
        var list = new List<string>();

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        foreach (var item in p)
        {
            var i = AssetDatabase.GUIDToAssetPath(item);
            list.Add(i);
            if (!_sceneToggle.ContainsKey(i))
                _sceneToggle.Add(i, false);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Toggle(_sceneToggle[i]);
            EditorGUILayout.LabelField(i);
            EditorGUILayout.EndHorizontal();
            
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("BAKE LIGHTS !") && !_baking)
        {
            _baking = true;
            BakeLights(list);
        }

        if(_baking)
        {
            EditorGUILayout.LabelField("Scenes Already Baked and Baking:");
            foreach (var item in _scenesBuilt)
            {
                var splitted = item.Split('/');
                EditorGUILayout.LabelField(splitted[splitted.Length - 1]);
            }
            if (GUILayout.Button("FORCE STOP BAKING !"))
            {
                Lightmapping.ForceStop();
                _baking = false;
                _sceneToggle = new Dictionary<string, bool>();
            }
        }
    }

    void BakeLights(List<string> list)
    {
        _realPaths = list.ToArray();
        _currentSceneBuildingIndex = 0;
        _scenesBuilt = new List<string>();
    }
}
