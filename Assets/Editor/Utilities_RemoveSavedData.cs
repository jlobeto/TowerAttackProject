using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Utilities_RemoveSavedData : EditorWindow
{
    [MenuItem("UTILITIES/Remove saved data")]
    static void Init()
    {
        var w = new WorldSelectorDevTools();
        w.DeleteSaveData(true);
    }
}
