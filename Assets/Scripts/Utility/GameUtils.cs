using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameUtils
{
    public const string CONFIG_PATH = "GameConfig/";
    public static string GetJson(string path)
    {
        var json = "";

        using (StreamReader r = new StreamReader(path))
        {
            json = r.ReadToEnd();
        }

        if (json == "")
            throw new System.Exception("Cound not GetJson at path " + path);

        return json;
    }

    public static T LoadConfig<T>(string fileName)
    {
        var json = GetJson(CONFIG_PATH + fileName);
        T data = JsonUtility.FromJson<T>(json);
        return data;
    }
}
