using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelGoal : MonoBehaviour, ILevelGoal
{
    public enum Goal {
        ArriveToEnd
    }

    public Goal goalType = Goal.ArriveToEnd;
    [Tooltip("When reach 0, user wins the level")]
    public int lives = 10;
    public Action OnGoalComplete = delegate { };
    protected int pCurrentLives;

    public int CurrentLives { get { return pCurrentLives; } }

    void Start()
    {
        pCurrentLives = lives;
    }

    public virtual void CheckGoal()
    {
        throw new System.NotImplementedException();
    }

    public virtual void UpdateGoal(int value)
    {
        throw new System.NotImplementedException();
    }
}
