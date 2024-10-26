using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<bool> ShootEvent;
    public Vector2 MoveDirection { get; private set; }

    private PlayerControls playerControls;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new();
            playerControls.Player.SetCallbacks(this);
        }

        playerControls.Player.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            ShootEvent?.Invoke(false);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseMenuUI pauseMenuUI = PauseMenuUI.Instance;

            if (pauseMenuUI == null) { return; }

            pauseMenuUI.PerformePauseButton();
        }
    }
}
