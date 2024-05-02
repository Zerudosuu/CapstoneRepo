using NodeCanvas.DialogueTrees;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    PlayerInputActions inputActions;
    public float raycastDistance = 10f;

    private Interactable interactable;
    private DialogueActor dialogueActor;
    public DialogueTreeController dialogue;

    public string InteractedObject;

    public bool canInteract = false;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Interact.started += DoInteract;
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Interact.started -= DoInteract;
        inputActions.Player.Disable();
    }

    private void DoInteract(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            if (dialogueActor != null) // Check if questGiver is not null
            {
                dialogue.StartDialogue();
            }
            else if (interactable != null) // Check if interactable is not null
            {
                PlayerQuest playerQuest = GetComponent<PlayerQuest>(); // Assuming PlayerQuest is on the same GameObject
                interactable.InteractWithObject(playerQuest.quest.quest);
            }
        }
    }

    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            canInteract = true;
            InteractedObject = other.name;

            interactable = other.gameObject.GetComponent<Interactable>();
        }
        else if (other.gameObject.GetComponent<DialogueActor>() != null)
        {
            canInteract = true;
            dialogueActor = other.gameObject.GetComponent<DialogueActor>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            canInteract = false;
            interactable = null;
        }
        else if (other.gameObject.GetComponent<QuestGiver>() != null)
        {
            dialogueActor = null;
        }
    }
}
