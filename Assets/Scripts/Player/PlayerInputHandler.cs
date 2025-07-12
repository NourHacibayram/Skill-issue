using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls playerControls;

    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool DashPressed { get; private set; }

    private void Awake()
    {
        try
        {
            playerControls = new PlayerControls();
            Debug.Log("PlayerControls initialized successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize PlayerControls: {e.Message}");
            Debug.LogError("Make sure you have created the PlayerControls Input Actions asset!");
        }
    }

    private void OnEnable()
    {
        if (playerControls != null)
        {
            EnableInputs();
        }
        else
        {
            Debug.LogError("PlayerControls is null in OnEnable! Cannot enable inputs.");
        }
    }

    private void OnDisable()
    {
        if (playerControls != null)
        {
            DisableInputs();
        }
    }

    private void Update()
    {
        // Reset JumpPressed after one frame
        if (JumpPressed)
        {
            JumpPressed = false;
        }
        
        // Reset DashPressed after one frame
        if (DashPressed)
        {
            DashPressed = false;
        }
    }

    private void EnableInputs()
    {
        if (playerControls?.Player == null)
        {
            Debug.LogError("PlayerControls.Player is null! Cannot enable inputs.");
            return;
        }

        var actions = playerControls.Player;
        actions.Enable();

        // Check if actions exist before subscribing
        if (actions.Move != null)
        {
            actions.Move.performed += OnMovePerformed;
            actions.Move.canceled += OnMoveCanceled;
        }
        else
        {
            Debug.LogError("Move action not found in PlayerControls!");
        }

        if (actions.Jump != null)
        {
            actions.Jump.performed += OnJumpPerformed;
            actions.Jump.canceled += OnJumpCanceled;
        }
        else
        {
            Debug.LogError("Jump action not found in PlayerControls!");
        }

        if (actions.Dash != null)
        {
            actions.Dash.performed += OnDashPerformed;
            actions.Dash.canceled += OnDashCanceled;
        }
        else
        {
            Debug.LogError("Dash action not found in PlayerControls!");
        }
    }

    private void DisableInputs()
    {
        if (playerControls?.Player == null)
        {
            return;
        }

        var actions = playerControls.Player;

        if (actions.Move != null)
        {
            actions.Move.performed -= OnMovePerformed;
            actions.Move.canceled -= OnMoveCanceled;
        }

        if (actions.Jump != null)
        {
            actions.Jump.performed -= OnJumpPerformed;
            actions.Jump.canceled -= OnJumpCanceled;
        }

        if (actions.Dash != null)
        {
            actions.Dash.performed -= OnDashPerformed;
            actions.Dash.canceled -= OnDashCanceled;
        }

        actions.Disable();
    }

    public void ResetInputs()
    {
        // Clear all input states to prevent stuck inputs
        MoveInput = Vector2.zero;
        JumpPressed = false;
        JumpHeld = false;
        DashPressed = false;
        
        Debug.Log("Input handler reset - all inputs cleared");
    }

    private void OnMovePerformed(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext context) => MoveInput = Vector2.zero;

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpPressed = true;
        JumpHeld = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        JumpHeld = false;
    }

    private void OnDashPerformed(InputAction.CallbackContext context) => DashPressed = true;
    private void OnDashCanceled(InputAction.CallbackContext context) => DashPressed = false;
}