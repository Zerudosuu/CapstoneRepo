using UnityEngine;

[System.Serializable]
public class ExperimentStep
{
    public string ObjectToInteract; // The object that needs to be interacted with
    public string RequiredObject; // The object that will be used to interact
    public int RequiredCount; // How many times this step needs to be completed
    public PriorityType priorityType; // Priority type of the stepthe step
}

public enum PriorityType
{
    One,
    Two,
    Three
}
