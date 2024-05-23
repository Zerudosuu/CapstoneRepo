using UnityEngine;
using UnityEngine.InputSystem;

public class DragDropScript : MonoBehaviour
{
    private GameObject draggedObject;
    private Vector3 mouseOffset;
    private float objectZCoord;
    private Collider objectCollider;
    private Vector3 originalPosition;

    public PlayerInputActions inputActions;
    public InteractionManager interactionManager;

    public bool isStarted => interactionManager.GameStarted;

    // Reference to the InteractionManager

    Rigidbody rb;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        originalPosition = gameObject.transform.position; // Store the original position
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Player.ClickDrop.performed += OnClick;
        inputActions.Player.ClickDrop.canceled += OnRelease;
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.ClickDrop.performed -= OnClick;
        inputActions.Player.ClickDrop.canceled -= OnRelease;
        inputActions.Player.Disable();
    }

    private void Start()
    {
        objectCollider = GetComponent<Collider>();
        interactionManager = GameObject.FindAnyObjectByType<InteractionManager>();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (!isStarted)
            return;
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    draggedObject = gameObject;
                    objectZCoord = Camera
                        .main.WorldToScreenPoint(draggedObject.transform.position)
                        .z;
                    mouseOffset =
                        draggedObject.transform.position - GetMouseWorldPosition(mousePosition);

                    // Disable the collider when dragging starts
                    objectCollider.enabled = false;
                }
            }
        }
        else
        {
            Debug.LogError("Mouse current is null.");
        }
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        // When the left mouse button is released, stop dragging the object
        if (draggedObject != null)
        {
            objectCollider.enabled = true; // Re-enable the collider when dragging stops
            draggedObject = null;
        }
    }

    void Update()
    {
        if (draggedObject != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = GetMouseWorldPosition(mousePosition);
            draggedObject.transform.position = mouseWorldPosition + mouseOffset;
        }
    }

    private Vector3 GetMouseWorldPosition(Vector2 mousePosition)
    {
        Vector3 mouseScreenPosition = new Vector3(mousePosition.x, mousePosition.y, objectZCoord);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        ExperimentStep currentStep = interactionManager.GetCurrentStep();

        if (
            currentStep != null
            && other.gameObject.name == currentStep.ObjectToInteract
            && gameObject.name == currentStep.RequiredObject
        )
        {
            if (currentStep.RequiredCount > 0)
            {
                currentStep.RequiredCount--;

                if (currentStep.RequiredCount <= 0)
                {
                    interactionManager.ProgressToNextStep();
                }

                // Reset the position and velocity
                gameObject.transform.position = originalPosition;
                this.rb.velocity = Vector3.zero;

                // Change the state of the other object
                ChangeState changeStateScript = other.gameObject.GetComponent<ChangeState>();
                if (changeStateScript != null)
                {
                    changeStateScript.ChangeStateObject();
                }
            }
        }
        else
        {
            // Invalid object interaction, reset position
            gameObject.transform.position = originalPosition;
            this.rb.velocity = Vector3.zero;
        }
    }
}
