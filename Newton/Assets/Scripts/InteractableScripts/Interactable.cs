using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI interactText;

    PlayerQuest playerQuest;

    void Start()
    {
        playerQuest = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerQuest>();
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

    public void InteractWithObject(List<string> questItems)
    {
        // Check if the gameObject is null (indicating it has been destroyed)
        if (gameObject == null)
        {
            return;
        }

        bool objectFound = false; // Flag to indicate if the object is found

        foreach (string questItem in questItems)
        {
            if (questItem == gameObject.name)
            {
                objectFound = true; // Set the flag to true
                print("Object found!");

                if (playerQuest != null && playerQuest.quest.isActive)
                {
                    playerQuest.quest.questGoal.Gathered();
                    if (playerQuest.quest.questGoal.IsReached())
                    {
                        playerQuest.quest.Complete();
                    }
                }
                break; // Exit the loop once the object is found
            }
        }

        // Check if the object is not relevant to the current quest and was not found
        if (!objectFound)
        {
            print(gameObject.name + " is not relevant to your current quest.");
        }
    }
}
