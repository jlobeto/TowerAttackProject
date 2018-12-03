using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayDevTools : MonoBehaviour
{
    public void OnWinLevel(int stars)
    {
        FindObjectOfType<Level>().ForceLevelWin(stars);
    }
}
