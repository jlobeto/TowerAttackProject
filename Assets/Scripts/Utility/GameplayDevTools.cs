using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayDevTools : MonoBehaviour
{
    private void Start()
    {
        var _gameManager = FindObjectOfType<GameManager>();
        if (!_gameManager.showDevTools)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnWinLevel(int stars)
    {
        FindObjectOfType<Level>().ForceLevelWin(stars);
    }
}
