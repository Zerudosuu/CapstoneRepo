using NodeCanvas.DialogueTrees;
using NodeCanvas.Tasks.Actions;
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

    public bool isInteracting = false;

    Vector3 originalVelocity;

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
                transform.LookAt(dialogueActor.transform.position);

                dialogue.StartDialogue();
            }
            else if (interactable != null) // Check if interactable is not null
            {
                PlayerQuest playerQuest = GetComponent<PlayerQuest>(); // Assuming PlayerQuest is on the same GameObject
                if (playerQuest != null)
                {
                    interactable.InteractWithObject(playerQuest.quest.quest);
                }
                else
                {
                    Debug.LogError(
                        "PlayerQuest component not found on the same GameObject as PlayerInteract."
                    );
                }
            }
        }
    }

    // private void LookAt(Transform target) // Ensure target is a Transform
    // {
    //     if (target != null)
    //     {
    //         Vector3 direction = target.position - transform.position;
    //         Quaternion lookRotation = Quaternion.LookRotation(direction);
    //         transform.rotation = Quaternion.Slerp(
    //             transform.rotation,
    //             lookRotation,
    //             Time.deltaTime * 5f
    //         ); // Smooth look-at (optional)
    //     }
    // }

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
            dialogue = other.gameObject.GetComponent<DialogueTreeController>();
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
            dialogue = null;
        }
    }
}
