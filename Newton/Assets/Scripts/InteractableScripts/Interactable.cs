using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI interactText;

    void Start()
    {
        interactText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.gameObject.SetActive(false);
        }
    }

    public void InteractWithObject(List<QuestRequiredItem> questItems)
    {
        // Check if the gameObject is null (indicating it has been destroyed)
        if (gameObject == null)
        {
            return;
        }

        PlayerQuest playerQuest = GameObject.FindAnyObjectByType<PlayerQuest>(); // Assuming PlayerQuest is on the same GameObject

        // Check if playerQuest is not null
        if (playerQuest == null)
        {
            Debug.LogError("PlayerQuest component not found.");
            return;
        }

        // Check if there is a current quest
        if (playerQuest.currentQuest == null)
        {
            print("There is no current quest");
            return;
        }

        bool objectFound = false; // Flag to indicate if the object is found

        foreach (var questItem in questItems)
        {
            if (questItem.ItemName == gameObject.name)
            {
                objectFound = true; // Set the flag to true

                foreach (var item in playerQuest.currentQuest.QuestRequiredItem)
                {
                    if (gameObject.name == item.ItemName)
                    {
                        print("Item found will add current amount");
                        item.questGoal.Gathered();

                        if (item.questGoal.IsReached())
                        {
                            print(item.ItemName + " is Reached!");
                            item.questGoal.isCollected = true;
                        }
                    }
                }
            }
        }

        // Check if the object is not relevant to the current quest and was not found
        if (!objectFound)
        {
            print(gameObject.name + " is not relevant to your current quest.");
        }

        // Check if all items are collected and complete the quest
        if (playerQuest.currentQuest.AreAllItemsCollected())
        {
            playerQuest.currentQuest.Complete();
        }
    }
}
