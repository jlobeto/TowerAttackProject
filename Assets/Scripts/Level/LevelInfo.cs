using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Will have only the data that is taken from the Json Config, so we can know what scene(for the moment is the scene) load,
/// what info the level has and more.
/// </summary>
[Serializable]
public class LevelInfo
{
    public int id;
    public string mode;//Normal,Hard
    /// <summary>
    /// 3 objetives: First is the minimun required to win the level, and the last is the maximum score to succed the level.
    /// </summary>
    public int[] objectives;
    public int[] currencyGainedByObjectives;
    public bool weatherEvents;
    public bool levelEvents;
}
