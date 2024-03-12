using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public Goaltype Goaltype;

    public int requiredAmount;
    public int CurrentAmount;

    public bool IsReached() 
    {
        return (CurrentAmount >= requiredAmount);
    
    }

    public void EnemyKilled() 
    {
        if (Goaltype == Goaltype.kill) 
        {
            CurrentAmount++;
        
        }
    
    }
    public void itemCollected()
    {
        if (Goaltype == Goaltype.Gathering)
        {
            CurrentAmount++;

        }

    }
    public void LocationReached()
    {
        if (Goaltype == Goaltype.Location)
        {
            CurrentAmount++;

        }

    }

}

public enum Goaltype 
{
    kill,
    Gathering,
    Location,

}
