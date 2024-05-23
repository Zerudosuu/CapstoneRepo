using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    public QuestTree quest;
    public QuestRequirements currentQuest;

    public bool questActive => currentQuest.isCompleted;

    public void AssignQuest(QuestRequirements quest)
    {
        currentQuest = quest;
        Debug.Log("Assigned quest: " + currentQuest.title);
    }
}
