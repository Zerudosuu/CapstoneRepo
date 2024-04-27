using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance; // Singleton instance

    public QuestExperiments[] currentQuests;
    public int currentQuestIndex = 0; // Index of the currently active quest

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    public bool IsItemOnQuestList(string itemName)
    {
        if (currentQuests == null || currentQuests.Length == 0)
        {
            return false; // No active quests
        }

        // Get the current quest
        var currentQuest = currentQuests[currentQuestIndex];

        // Iterate over all items in the current quest
        foreach (var item in currentQuest.questItems)
        {
            // Check if the item name matches
            if (item.ItemName == itemName)
            {
                // Mark the item as collected
                item.isCollected = true;

                // Check if all items in the current quest are collected
                if (currentQuest.questItems.All(questItem => questItem.isCollected))
                {
                    currentQuest.isCompleted = true; // Mark the quest as completed
                    AdvanceQuest(); // Advance to the next quest
                }

                return true;
            }
        }

        return false; // No matching item found
    }

    public void AdvanceQuest()
    {
        if (currentQuests == null || currentQuests.Length == 0)
        {
            return; // No quests to advance
        }

        // Check if current quest is completed
        if (currentQuests[currentQuestIndex].IsCompleted())
        {
            currentQuestIndex++; // Move to the next quest
        }

        // Ensure we stay within bounds
        currentQuestIndex = Mathf.Clamp(currentQuestIndex, 0, currentQuests.Length - 1);
    }

    // Methods to update currentQuests based on quest progress (implement as needed)
}
