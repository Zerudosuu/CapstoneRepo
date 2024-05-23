using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Story", menuName = "Try")]
public class QuestTree : ScriptableObject
{
    public List<QuestRequirements> story = new();
}

[System.Serializable]
public class QuestRequirements
{
    public bool isCompleted;
    public bool isActive;
    public string title;
    public string description;

    public int ExperienceReward;
    public int CoinReward;

    public int EnergyReward;

    public List<QuestRequiredItem> QuestRequiredItem = new();

    public void Complete()
    {
        isCompleted = true;
    }

    public void ResetQuest()
    {
        isCompleted = false;
        isActive = false;
    }

    public bool AreAllItemsCollected()
    {
        foreach (var item in QuestRequiredItem)
        {
            if (!item.questGoal.isCollected)
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class QuestRequiredItem
{
    public string ItemName;
    public int ItemQuantity;

    public QuestGoal questGoal;
}
