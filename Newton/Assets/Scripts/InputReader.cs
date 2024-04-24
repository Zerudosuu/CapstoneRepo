using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader", fileName = "InputReader")]
public class InputReader : ScriptableObject
{
    [SerializeField]
    private InputActionAsset _actionAsset;

    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction JumpEvent;
    public event UnityAction JumpCancelledEvent;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    void OnEnable()
    {
        _moveAction = _actionAsset.FindAction("Move");
        _jumpAction = _actionAsset.FindAction("Jump");

        _moveAction.started += OnMove;
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;

        _jumpAction.started += OnJump;
        _jumpAction.performed += OnJump;
        _jumpAction.canceled += OnJump;

        _moveAction.Enable();
        _jumpAction.Enable();
    }

    void OnDisable()
    {
        _moveAction.started -= OnMove;
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;

        _jumpAction.started -= OnJump;
        _jumpAction.performed -= OnJump;
        _jumpAction.canceled -= OnJump;

        _moveAction.Disable();
        _jumpAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (JumpEvent != null && context.started)
        {
            JumpEvent.Invoke();
        }

        if (JumpCancelledEvent != null && context.canceled)
        {
            JumpCancelledEvent.Invoke();
        }
    }
}
