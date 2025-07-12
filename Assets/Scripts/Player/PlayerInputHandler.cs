/* using UnityEngine;
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
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    private void OnDisable()
    {
        DisableInputs();
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
        var actions = playerControls.Player;
        actions.Enable();

        actions.Move.performed += OnMovePerformed;
        actions.Move.canceled += OnMoveCanceled;

        actions.Jump.performed += OnJumpPerformed;
        actions.Jump.canceled += OnJumpCanceled;

        actions.Dash.performed += OnDashPerformed;
        actions.Dash.canceled += OnDashCanceled;
    }

    private void DisableInputs()
    {
        var actions = playerControls.Player;

        actions.Move.performed -= OnMovePerformed;
        actions.Move.canceled -= OnMoveCanceled;

        actions.Jump.performed -= OnJumpPerformed;
        actions.Jump.canceled -= OnJumpCanceled;

        actions.Dash.performed -= OnDashPerformed;
        actions.Dash.canceled -= OnDashCanceled;

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
} */