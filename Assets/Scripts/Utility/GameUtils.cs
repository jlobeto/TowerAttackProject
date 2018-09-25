using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameUtils
{
    public const string CONFIG_PATH = "GameConfig/";
    public const string MINION_CONFIG_PATH = "GameConfig/Minions/";
    public const string TOWER_CONFIG_PATH = "GameConfig/Towers/";

    public static string GetJson(string path)
    {
        var json = "";

        using (StreamReader r = new StreamReader(path))
        {
            json = r.ReadToEnd();
        }

        if (json == "")
            throw new Exception("Cound not GetJson at path " + path);

        return json;
    }

    public static T LoadConfig<T>(string fileName, string path = CONFIG_PATH)
    {
        var json = GetJson(path + fileName);
        T data = JsonUtility.FromJson<T>(json);
        return data;
    }
}
