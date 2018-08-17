using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArriveToEndGoal : LevelGoal
{
    
    public override void UpdateGoal(int value)
    {
        Debug.Log("update goal");
        Debug.Log(pCurrentLives  +"befone");
        pCurrentLives += value;
        Debug.Log(pCurrentLives +" after");
        if (pCurrentLives > 0)
            Debug.Log("Goal Updated. Remaining lives : " + pCurrentLives);

        CheckGoal(); 
    }


    public override void CheckGoal()
    {
        if (pCurrentLives == 0)
            OnGoalComplete();
    }
}
