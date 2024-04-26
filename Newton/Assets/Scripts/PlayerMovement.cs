using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

public class PlayerMovement : MonoBehaviour
{
    #region InputActionsFields
    PlayerInputActions inputActions;
    InputAction move;

    #endregion

    #region  MovementField
    private Rigidbody rbody;

    [SerializeField]
    private float movementForce = 1f;

    [SerializeField]
    private float jumpForce = 5f;

    [SerializeField]
    private float maxSpeed = 6f;

    private Vector3 forceDirection = Vector3.zero;

    #endregion

    [SerializeField]
    private Camera playerCamera;

    void Awake()
    {
        rbody = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
    }

    #region Subsribe/Unsubscribe/InputActionMethods
    void OnEnable()
    {
        inputActions.Player.Jump.started += DoJump;
        move = inputActions.Player.Move;
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Jump.started -= DoJump;
        move = inputActions.Player.Move;
        inputActions.Player.Disable();
    }

    private void DoJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    #endregion



    void FixedUpdate()
    {
        forceDirection +=
            move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection +=
            move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        // If the player is continuously pressing the button it will not set to Vector3.Zero
        rbody.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        rbody.useGravity = true;

        Vector3 horizontalVelocity = rbody.velocity;
        horizontalVelocity.y = 0f;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rbody.velocity =
                horizontalVelocity.normalized * maxSpeed + Vector3.up * rbody.velocity.y;
        }

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rbody.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rbody.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rbody.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            return true;
        else
            return false;
    }
}
