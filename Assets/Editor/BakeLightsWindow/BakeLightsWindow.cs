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
        _lightMapResIndex = EditorGUILayout.Popup(_lightMapResIndex, _lightmapResolution);

        GUILayout.Space(1);


        if (GUILayout.Button("BAKE LIGHTS !") && !_baking)
        {
            _baking = true;
            BakeLights();
        }

        if(_baking)
        {
            EditorGUILayout.LabelField("Scenes Already Baked and Baking:");
            foreach (var item in _scenesBuilt)
            {
                var splitted = item.Split('/');
                EditorGUILayout.LabelField(splitted[splitted.Length - 1]);
            }
            if (GUILayout.Button("STOP BAKING !"))
            {
                Lightmapping.ForceStop();
                _baking = false;
            }
        }
    }

    void BakeLights()
    {
        var paths = new string[] { _pathToScenes };

        var p = AssetDatabase.FindAssets("t:scene", paths);
        var list = new List<string>();
        
        foreach (var item in p)
        {
            var i = AssetDatabase.GUIDToAssetPath(item);
            list.Add(i);
        }

        _realPaths = list.ToArray();
        _currentSceneBuildingIndex = 0;
        _scenesBuilt = new List<string>();
    }
}
