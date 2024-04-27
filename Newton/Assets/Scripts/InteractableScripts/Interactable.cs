using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Enemy,
        Item
    }

    [SerializeField]
    private TextMeshProUGUI interactText;
    public InteractableType interactableType;

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

    public void InteractWithObject(string name)
    {
        // Check if the gameObject is null (indicating it has been destroyed)
        if (gameObject == null)
        {
            return;
        }

        // Check for Quest Manager instance (avoid null reference errors)
        if (QuestManager.Instance != null)
        {
            if (QuestManager.Instance.IsItemOnQuestList(name)) // Check quest relevance
            {
                print(gameObject.name + " interaction triggered!");

                // Destroy the gameObject after a successful interaction
                Destroy(gameObject);
            }
            else
            {
                print(gameObject.name + " is not relevant to your current quest.");
            }
        }
        else
        {
            Debug.LogError("QuestManager not found! Ensure it's attached to a GameObject.");
        }
    }
}
