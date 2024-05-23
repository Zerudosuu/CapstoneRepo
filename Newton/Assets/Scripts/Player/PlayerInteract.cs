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

    public bool interactedWithStore;
    public bool interactingWithArcade;
    Vector3 originalVelocity;

    PlayerQuest playerQuest;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        playerQuest = GetComponent<PlayerQuest>();
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
                // Assuming PlayerQuest is on the same GameObject
                if (playerQuest.currentQuest != null)
                {
                    interactable.InteractWithObject(playerQuest.currentQuest.QuestRequiredItem);
                }
                else
                {
                    Debug.LogError(
                        "PlayerQuest component not found on the same GameObject as PlayerInteract."
                    );
                }
            }
            else if (interactedWithStore)
            {
                UIController uIController = GameObject.FindObjectOfType<UIController>();

                uIController.OpenStore();
            }
            else if (interactingWithArcade)
            {
                UIController uIController = GameObject.FindObjectOfType<UIController>();
                uIController.EnableCameraArcade();
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
        else if (other.gameObject.GetComponent<Store>() != null)
        {
            canInteract = true;
            interactedWithStore = true;
            print("hey");
        }
        else if (other.gameObject.GetComponent<Arcade>() != null)
        {
            canInteract = true;
            interactingWithArcade = true;
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
        else if (other.gameObject.GetComponent<Store>() != null)
        {
            interactedWithStore = false;
            canInteract = false;
        }
        else if (other.gameObject.GetComponent<Arcade>() != null)
        {
            interactingWithArcade = false;
            canInteract = false;
        }
    }
}
