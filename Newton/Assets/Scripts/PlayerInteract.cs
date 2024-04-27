using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    PlayerInputActions inputActions;
    public float raycastDistance = 10f;

    private Interactable interactable;

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
            interactable.InteractWithObject(InteractedObject);
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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            canInteract = false;
        }
    }
}
