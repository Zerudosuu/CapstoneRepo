using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isCompleted;
    public bool isActive;
    public string title;
    public string description;

    public List<string> quest;

    public QuestGoal questGoal;

    public void Complete()
    {
        isActive = false;
    }
}
