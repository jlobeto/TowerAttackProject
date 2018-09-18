using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelNodesLoader
{

    public LevelNodeList LevelInfoList { get { return _levelNodeList; } }

    LevelNodeList _levelNodeList;

    string jsonName = "Assets/GameConfig/LevelInfoConfig.json";

    public LevelNodesLoader()
    {
        var json = GetJson();
        _levelNodeList = JsonUtility.FromJson<LevelNodeList>(json);
    }

    string GetJson()
    {
        var json = "";

        /*using (StreamReader r = new StreamReader(jsonName))
        {
            json = r.ReadToEnd();
        }*/
        TextAsset txtAsset = (TextAsset)Resources.Load("LevelInfoConfig", typeof(TextAsset));
        return txtAsset.text;
        if (json == "")
        {
            jsonName = "LevelInfoConfig.json";
            
            using (StreamReader r = new StreamReader(jsonName))
            {
                json = r.ReadToEnd();
            }
        }


        return json;
    }
}
