using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MinionStoreData
{
    public string type;
    public int currencyValue;
    /// <summary>
    /// Stars that the player has to have in progress to unlock this minion on the store.
    /// </summary>
    public int starsNeedToUnlock;
    public string[] info;
    public string skillInfo;
    public string passiveInfo;
}
