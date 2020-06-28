using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BakeLightsWindow : EditorWindow
{
    string _pathToScenes = "Assets/Scenes/Levels";
    string[] _lightmapResolution = new string[] { "1024", "2048" };
    int _lightMapResIndex;

    [MenuItem("UTILITIES/Bake Scenes Lights")]
    static void Init()
    {

        BakeLightsWindow window = (BakeLightsWindow)EditorWindow.GetWindow(typeof(BakeLightsWindow));
        window.Show();
    }

    void OnGUI()
    {

        _pathToScenes = EditorGUILayout.TextField("Path To Scenes", _pathToScenes);
        GUILayout.Space(1);
        _lightMapResIndex = EditorGUILayout.Popup(_lightMapResIndex, _lightmapResolution);

        GUILayout.Space(1);


        if (GUILayout.Button("BAKE LIGHTS !"))
        {
            BakeLights();
        }
    }

    void BakeLights()
    {
        var paths = new string[] { _pathToScenes };
        LightmapEditorSettings.maxAtlasSize = int.Parse(_lightmapResolution[_lightMapResIndex]);
        LightmapEditorSettings.lightmapper = LightmapEditorSettings.Lightmapper.ProgressiveCPU;
        LightmapEditorSettings.mixedBakeMode = MixedLightingMode.Subtractive;
        LightmapEditorSettings.lightmapsMode = LightmapsMode.NonDirectional;

        var p = AssetDatabase.FindAssets("t:scene", paths);
        var list = new List<string>();
        foreach (var item in p)
        {
            var i = AssetDatabase.GUIDToAssetPath(item);
            list.Add(i);
        }

        Lightmapping.BakeMultipleScenes(list.ToArray());
    }
}
