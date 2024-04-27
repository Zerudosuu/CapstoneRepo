using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest", fileName = "QuestItem")]
public class QuestExperiments : ScriptableObject
{
    public QuestItem[] questItems;
    public bool isCompleted;

    public bool IsCompleted()
    {
        if (questItems == null || questItems.Length == 0)
        {
            return true; // No items, consider complete
        }

        // Check if all items are collected
        foreach (var item in questItems)
        {
            if (!item.isCollected)
            {
                return false; // Not all items collected, quest is not complete
            }
        }

        return true; // All items collected, quest is complete
    }
}

[System.Serializable]
public class QuestItem
{
    public string ItemName;
    public bool isCollected;
}
