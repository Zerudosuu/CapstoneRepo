using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private InputReader Input;

    private Rigidbody playerRigidbody;

    [SerializeField]
    private float movementForce = 1f;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    private bool isJumping;

    /// <summary>

    /// </summary>
    void OnEnable()
    {
        Input.MoveEvent += OnMove;
        Input.JumpEvent += OnJump;
        Input.JumpCancelledEvent += OnJumpCancelled;
    }

    void OnDisable()
    {
        Input.MoveEvent -= OnMove;
        Input.JumpEvent -= OnJump;
        Input.JumpCancelledEvent -= OnJumpCancelled;
    }

    private void OnMove(Vector2 movement)
    {
        forceDirection = new Vector3(movement.x, 0f, movement.y) * movementForce;
    }

    private void OnJump()
    {
        isJumping = true;
    }

    private void OnJumpCancelled()
    {
        isJumping = false;
    }

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Jump();
        Moved();
    }

    private void Moved()
    {
        playerRigidbody.velocity = Vector3.ClampMagnitude(
            playerRigidbody.velocity + forceDirection,
            maxSpeed
        );
        forceDirection = Vector3.zero;
    }

    void Jump()
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return isJumping = true;
        else
            return isJumping = false;
    }
}
