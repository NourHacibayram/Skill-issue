using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerControls.IPlayerActions
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayerMask;

    private Rigidbody2D rb;
    private PlayerControls playerControls;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isDashing;
    private float dashTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerControls = new PlayerControls();
        playerControls.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        CheckGrounded();
        HandleDash();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayerMask);
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    void HandleDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
            }
        }
    }

    // Input System Callbacks
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
            Vector2 dashDirection = moveInput.x != 0 ? new Vector2(moveInput.x, 0) : Vector2.right;
            rb.linearVelocity = dashDirection.normalized * dashSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }


    public void ZeroVelocity()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    
    public void ClearInputs()
    {
        // Clear any input-related variables
        // Based on your existing code structure:
        if (playerControls != null)
        {
            // Reset input values if needed
        }
    }
    
    public void SetSpawnPosition(Vector2 position)
    {
        transform.position = position;
        ZeroVelocity();
        // Reset any movement states
        /*
        if (stateMachine != null && idleState != null)
        {
            stateMachine.ChangeState(idleState);
        }
        */
    }
    
    public void OnRoomTransition()
    {
        // Called when transitioning between rooms
        ZeroVelocity();
        ClearInputs();
    }
}