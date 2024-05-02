using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;

    public PlayerQuest player;

    #region GUISection
    public GameObject questWindow;

    public GameObject questItemContainer;

    public GameObject itemNamePrefab; // Prefab for the item name

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    #endregion

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = "Description: " + quest.description;

        // Clear existing items in the container
        foreach (Transform child in questItemContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Instantiate itemNamePrefab for each quest item and set text
        foreach (string questItemText in quest.quest)
        {
            GameObject newItemName = Instantiate(itemNamePrefab, questItemContainer.transform);
            TextMeshProUGUI textComponent = newItemName.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = questItemText;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on itemNamePrefab.");
            }
        }
    }

    public void AcceptQuest()
    {
        questWindow.SetActive(false);
        quest.isActive = true;

        //give to player
        player.quest = quest;
        quest.questGoal.reqAmount = quest.quest.Count;
    }
}
