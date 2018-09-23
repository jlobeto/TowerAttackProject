using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelNodesLoader
{

    public LevelNodeList LevelInfoList { get { return _levelNodeList; } }

    LevelNodeList _levelNodeList;

    string jsonName = "LevelInfoConfig.json";

    public LevelNodesLoader()
    {
        _levelNodeList = GameUtils.LoadConfig<LevelNodeList>(jsonName);
    }
}
