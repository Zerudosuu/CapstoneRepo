using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeState : MonoBehaviour
{
    public List<GameObject> GameObjects;
    public int currentIndex = 0;

    public void ChangeStateObject()
    {
        if (GameObjects == null || GameObjects.Count == 0)
        {
            Debug.LogWarning("No GameObjects in the list.");
            return;
        }

        // Set the current active object to inactive
        GameObjects[currentIndex].SetActive(false);

        // Increment the index and wrap around if necessary
        currentIndex = (currentIndex + 1) % GameObjects.Count;

        // Set the new current object to active
        GameObjects[currentIndex].SetActive(true);
    }
}
