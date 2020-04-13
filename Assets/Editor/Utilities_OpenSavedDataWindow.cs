using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Utilities_OpenSavedDataWindow : EditorWindow
{
    [MenuItem("UTILITIES/Open saved data window")]
    static void Init()
    {
        

        if (Directory.Exists(Application.persistentDataPath))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = Application.persistentDataPath,
                FileName = "explorer.exe"
            };
            Process.Start(Application.persistentDataPath);
        }
    }
}

