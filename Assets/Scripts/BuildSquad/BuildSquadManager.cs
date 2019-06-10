using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSquadManager : MonoBehaviour
{
    public List<SquadSelectedMinion> selectedButtons;

    GameManager _gm;
    User _user;

    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        _user = _gm.User;
    }

    
    void Update()
    {
        
    }
}
