using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLevelsList", menuName = "ScriptableObjects/SceneLevelList")]
public class SceneLevelListSO : ScriptableObject
{
    public List<string> levelsScene;
}
