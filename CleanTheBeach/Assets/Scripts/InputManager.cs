using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _jumpAction;
    private InputAction _sprintAction;
    
    private void Start()
    {
        // Lock and hide cursor for First Person 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Get the PlayerInput component
        _playerInput = GetComponent<PlayerInput>();
        _jumpAction = _playerInput.actions["Jump"];
        _sprintAction = _playerInput.actions["Sprint"];
    }

    private void Update()
    {
        InputValuesManager.IsJumping = _jumpAction.IsPressed();
        InputValuesManager.IsSprinting = _sprintAction.IsPressed();
    }

    private void OnMove(InputValue value)
    {
        var data = value.Get<Vector2>();
        InputValuesManager.MoveDirection = new Vector2(data.x, data.y);
    }
    
    private void OnLook(InputValue value)
    {
        var data = value.Get<Vector2>();
        InputValuesManager.LookRotation = data;
    }
}
