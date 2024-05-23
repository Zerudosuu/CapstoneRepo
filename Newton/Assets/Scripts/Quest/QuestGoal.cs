using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int reqAmount;
    public int curAmount;
    public bool isCollected;

    public bool IsReached()
    {
        return (curAmount >= reqAmount);
    }

    public void Gathered()
    {
        if (goalType == GoalType.Gathering)
        {
            curAmount++;
        }
    }

    public void ResetGoal()
    {
        curAmount = 0;
        isCollected = false;
    }
}

public enum GoalType
{
    Gathering,
}
