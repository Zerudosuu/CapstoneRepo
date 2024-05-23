using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIExperimentController : MonoBehaviour
{
    List<VisualElement> visualElements = new(); // Array to store all visual elements
    string hoveredObjectName = ""; // Tracks the currently hovered object name
    float hoverStartTime = 0f; // Tracks the start time of the hover (optional)

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement elasticString = root.Q<VisualElement>("ElasticString");
        VisualElement scissor = root.Q<VisualElement>("Scissor");
        VisualElement shoeBox = root.Q<VisualElement>("ShoeBox");

        visualElements.Add(elasticString);
        visualElements.Add(scissor);
        visualElements.Add(shoeBox);

        print(visualElements[0].name);
        print(visualElements[1].name);
        print(visualElements[2].name);
    }

    void Update()
    {
        // Get the main camera (optional, can be cached if needed frequently)
        Camera camera = Camera.main;

        // Convert mouse position on screen to a ray in world space
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // Declare a variable to store the raycast hit information
        RaycastHit hit;

        // Perform a raycast and check if it hits anything
        if (Physics.Raycast(ray, out hit))
        {
            string objectName = hit.collider.gameObject.name;

            // Update hovered object name
            hoveredObjectName = objectName;

            // Update hover time (optional)
            hoverStartTime += Time.deltaTime;

            // Check if hover time exceeds 2 seconds (implement your desired duration)
            if (hoverStartTime >= 2f)
            {
                EnableVisualElement(objectName);
                hoverStartTime = 0f; // Reset hover time
            }
        }
        else
        {
            // Reset hovered object name and hide any previously shown element
            hoveredObjectName = "";
            HideAllVisualElements();
        }
    }

    void EnableVisualElement(string objectName)
    {
        // Loop through all visual elements
        foreach (var element in visualElements)
        {
            if (element.name == objectName)
            {
                element.style.display = DisplayStyle.Flex;
                return; // Exit the loop after finding and enabling the matching element
            }
        }

        // Log a warning if the object name doesn't match any visual element
        Debug.LogWarning(
            "Object with name '" + objectName + "' not found in visual elements array."
        );
    }

    void HideAllVisualElements()
    {
        // Loop through all visual elements and set their display to None
        foreach (var element in visualElements)
        {
            element.style.display = DisplayStyle.None;
        }
    }
}
