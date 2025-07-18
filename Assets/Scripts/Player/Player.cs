using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputHandler inputHandler { get; private set; }

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    #endregion

    [Header("Collision")]
    public bool grounded;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public float wallCheckDistance;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public BoxCollider2D groundCheck;

    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float maxJumps;
    public float doubleJumpForce;

    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }

    [Header("Wall Climb")]
    
    [HideInInspector]
    public bool ledgeDetected;
    [SerializeField] private Vector2 wallClimbOffset1;
    [SerializeField] private Vector2 wallClimbOffset2;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;
    private bool canGrabLedge = true;
    private bool canClimb;
    
    

    public SkillManager skill { get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerDoubleJumpState doubleJumpState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerWallClimbState wallClimbState { get; private set; }
    public PlayerWallClimbIdleState wallClimbIdleState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    #endregion

    #region Audio
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip fallSound;
    [SerializeField] private AudioClip deathSound;
    #endregion

    public bool isBusy { get; private set; } = false;
    public float facingDirection { get; set; } = 1;
    public bool facingRight = true;

    private bool wasGrounded = true; // Track previous grounded state


    public virtual void Awake()
    {
        // Get or add the input handler component
        inputHandler = GetComponent<PlayerInputHandler>();
        if (inputHandler == null)
        {
            inputHandler = gameObject.AddComponent<PlayerInputHandler>();
        }

        stateMachine = gameObject.AddComponent<PlayerStateMachine>();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        doubleJumpState = new PlayerDoubleJumpState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        wallClimbState = new PlayerWallClimbState(this, stateMachine, "WallClimb");
        wallClimbIdleState = new PlayerWallClimbIdleState(this, stateMachine, "WallClimbIdle");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
    }

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }

    public virtual void Update()
    {
        stateMachine.currentState.Update();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        // Check for landing (was in air, now grounded)
        if (!wasGrounded && isGrounded())
        {
            PlayFallSound();
        }

        // Update previous grounded state
        wasGrounded = isGrounded();

        // Safety check for stuck states

        CheckForLedge();
    }

    #region Play Audio
    private void PlayFallSound()
    {
        if (fallSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fallSound);
        }
    }
    public void PlayJumpSound()
    {
        if (jumpSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }
    
    public void PlayDeathSound()
    {
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
    #endregion


    #region Collision

    public bool isGrounded()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, whatIsGround).Length > 0;
        return grounded;
    }

    public bool IsWallDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

        if (hit.collider != null)
        {
            // Skip if it's a one-way platform
            if (hit.collider.CompareTag("OneWay") || hit.collider.GetComponent<PlatformEffector2D>() != null)
                return false;

            return true;
        }

        return false;
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + wallClimbOffset1;
            climbOverPosition = ledgePosition + wallClimbOffset2;

            canClimb = true;

            if (canClimb)
                transform.position = climbBegunPosition;
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * facingDirection * wallCheckDistance);
    }
    #endregion

    #region Input
    public Vector2 GetMoveInput()
    {
        // Add null check to prevent NullReferenceException
        if (inputHandler == null)
        {
            Debug.LogWarning("InputHandler is null in Player.GetMoveInput()");
            return Vector2.zero;
        }
        return inputHandler.MoveInput;
    }

    public bool GetJumpPressed()
    {
        // Add null check to prevent NullReferenceException
        if (inputHandler == null)
        {
            Debug.LogWarning("InputHandler is null in Player.GetJumpPressed()");
            return false;
        }
        return inputHandler.JumpPressed;
    }

    public bool GetDashPressed()
    {
        // Add null check to prevent NullReferenceException
        if (inputHandler == null)
        {
            Debug.LogWarning("InputHandler is null in Player.GetDashPressed()");
            return false;
        }
        return inputHandler.SkillPressed;
    }

    public bool GetWallClimbPressed()
    {
        // Add null check to prevent NullReferenceException
        if (inputHandler == null)
        {
            Debug.LogWarning("InputHandler is null in Player.GetWallClimbPressed()");
            return false;
        }
        return inputHandler.WallClimbPressed;
    }

    public bool GetWallClimbHeld()
    {
        // Add null check to prevent NullReferenceException
        if (inputHandler == null)
        {
            Debug.LogWarning("InputHandler is null in Player.GetWallClimbHeld()");
            return false;
        }
        
        // Use the proper held input from the Input System
        return inputHandler.WallClimbHeld;
    }
    #endregion

    public IEnumerator BusyFor(float _time)
    {
        isBusy = true;
        yield return new WaitForSeconds(_time);
        isBusy = false;
    }

    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }


    #region Velocity
    public void ZeroVelocity()
    {
        rb.linearVelocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    
    public void SetDashDirection(float direction)
    {
        dashDirection = direction;
    }
    #endregion

    #region Flip
    public void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (facingRight && _x < 0)
        {
            Flip();
        }
        else if (!facingRight && _x > 0)
        {
            Flip();
        }
    }
    #endregion
}