using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public ExperimentFlow experimentFlow; // Reference to the ExperimentFlow ScriptableObject
    public int currentStepIndex = 0; // Track the current step in the experiment flow

    [SerializeField]
    private GameObject CameraFocusOnObject;

    [SerializeField]
    public DialogueTreeController dialogue;

    public bool GameStarted = false;

    public bool GameFinished = false;

    // Method to get the current step

    void Start()
    {
        CameraFocusOnObject.SetActive(false);

        dialogue.StartDialogue();
    }

    public ExperimentStep GetCurrentStep()
    {
        if (currentStepIndex < experimentFlow.Steps.Count)
        {
            return experimentFlow.Steps[currentStepIndex];
        }
        else
        {
            return null; // No more steps
        }
    }

    // Method to progress to the next step
    public void ProgressToNextStep()
    {
        if (currentStepIndex < experimentFlow.Steps.Count - 1)
        {
            currentStepIndex++;
            print("CurrentIndex: " + currentStepIndex + "");
        }
        else
        {
            print("Game is finished");
            CameraFocusOnObject.SetActive(true);
            GameFinished = true;
        }
    }
}
