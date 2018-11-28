using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelNodesLoader
{

    public GenericListJsonLoader<LevelInfo> LevelInfoList { get { return _levelNodeList; } }

    GenericListJsonLoader<LevelInfo> _levelNodeList;

    string jsonName = "LevelInfoConfig.json";

    public LevelNodesLoader()
    {
        _levelNodeList = GameUtils.LoadConfig <GenericListJsonLoader<LevelInfo>>(jsonName);
    }
}
