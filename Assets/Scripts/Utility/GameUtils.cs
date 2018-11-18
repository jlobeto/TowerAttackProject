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

	enum Target{
		resources,
		external
	}

	static Target _target = Target.resources;

    public static string GetJson(string path)
    {
        var json = "";

		if (_target == Target.resources) 
		{
			path = path.Substring (0, path.IndexOf('.'));
			TextAsset txtAsset = (TextAsset)Resources.Load (path, typeof(TextAsset));
			json = txtAsset.text;
		} 
		else 
		{
			using (StreamReader r = new StreamReader(path))
			{
				json = r.ReadToEnd();
			}	
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
