using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.Rendering;

public class Player : MonoBehaviour
{



    public bool isBusy { get; private set; }

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer spriteRenderer;
    public PlayerInputHandler inputHandler { get; private set; }
    public CapsuleCollider2D col { get; private set; }
    #endregion

    #region Custom Physics
    [SerializeField] private ScriptableStats _stats;
    public ScriptableStats Stats => _stats;
    private FrameInput _frameInput;
    public Vector2 _frameVelocity; // Changed from private to public
    private bool _cachedQueryStartInColliders;
    private float _time;

    // Custom physics state
    private float _frameLeftGrounded = float.MinValue;
    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
    private bool CanUseCoyote => _coyoteUsable && !grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;
    
    #endregion

    [Header("Collision")]
    public bool grounded;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected BoxCollider2D groundCheck;

    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Double Jump")]
    public bool hasUsedDoubleJump = false;

    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;

    public float lastImageXpos;
    public float lastImageYpos;
    public float dashDirection { get; private set; }
    public bool canDash = true;
    public int maxDashes = 1;
    public int currentDashes;
    public float disntaceBetweenImages;

    [Header("Particles")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;



    public float facingDirection { get; set; } = 1;
    protected bool facingRight = true;

    public SkillManager skill { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDoubleJumpState doubleJumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    // public PlayerDeathState deathState { get; private set; }
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        inputHandler = GetComponent<PlayerInputHandler>();

        if (col.sharedMaterial == null)
        {
            PhysicsMaterial2D playerMaterial = new PhysicsMaterial2D("PlayerMaterial");
            playerMaterial.friction = 0f;
            playerMaterial.bounciness = 0f;
            col.sharedMaterial = playerMaterial;
        }

        if (inputHandler == null)
        {
            Debug.LogWarning("Adding PlayerInputHandler dynamically to player.");
            inputHandler = gameObject.AddComponent<PlayerInputHandler>();
        }

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        stateMachine = gameObject.AddComponent<PlayerStateMachine>();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        doubleJumpState = new PlayerDoubleJumpState(this, stateMachine, "DoubleJump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        // deathState = new PlayerDeathState(this, stateMachine, "Death");
    }

    private void Start()
    {
        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
        currentDashes = maxDashes;

    }

    private void Update()
    {
        _time += Time.deltaTime;
        GatherCustomInput();

        stateMachine.currentState.Update();
        CheckIfGrounded();

        if (IsWallDetected())
        {
        }
    }

    private void FixedUpdate()
    {
        CheckCustomCollisions();
        HandleCustomJump();
        HandleCustomDirection();
        HandleCustomGravity();
        ApplyCustomMovement();
    }

    #region Custom Physics Methods
    private void GatherCustomInput()
    {
        _frameInput = new FrameInput
        {
            JumpDown = GetJumpPressed(),
            JumpHeld = inputHandler.JumpHeld,
            Move = GetMoveInput()
        };

        if (_stats.SnapInput)
        {
            _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
            _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        }

        if (_frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }
    }

    private void CheckCustomCollisions()
    {
        Physics2D.queriesStartInColliders = false;

        // Use slightly smaller collision bounds to avoid corner catching
        Vector2 colliderSize = new Vector2(col.size.x * 0.9f, col.size.y);

        bool groundHit = Physics2D.CapsuleCast(col.bounds.center, colliderSize, col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
        bool ceilingHit = Physics2D.CapsuleCast(col.bounds.center, colliderSize, col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        bool wasGrounded = grounded;
        grounded = groundHit;        if (!wasGrounded && groundHit)
        {
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            RefreshDash();
            
            // Play landing particle effect
            if (jumpParticle != null)
            {
                jumpParticle.Play();
            }
            
            Debug.Log("Dash refreshed - landed on ground!");
        }
        else if (wasGrounded && !groundHit)
        {
            _frameLeftGrounded = _time;
            Debug.Log("Left ground - dash will not refresh until landing again");
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }

    private void RefreshDash()
    {
        canDash = true;
        currentDashes = maxDashes;
    }


    private void HandleCustomJump()
    {
        if (!_endedJumpEarly && !grounded && !_frameInput.JumpHeld && rb.linearVelocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (grounded || CanUseCoyote) ExecuteCustomJump();

        _jumpToConsume = false;
    }
    // ...existing code...
    private void ExecuteCustomJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;

        // Set vertical jump velocity
        _frameVelocity.y = _stats.JumpPower;

        // Check if player is giving horizontal input
        if (_frameInput.Move.x != 0)
        {
            // Player is moving - give horizontal boost for distance
            _frameVelocity.x = _frameInput.Move.x * _stats.MaxSpeed * 2.5f;
        }
        else
        {
            // Player only pressed space - just jump straight up with minimal horizontal movement
            _frameVelocity.x = 0f; // No horizontal velocity for straight up jump
        }

        Debug.Log($"Jump executed - Input: {_frameInput.Move.x}, Final velocity: {_frameVelocity}");
    }


    private void HandleCustomGravity()
    {
        // Don't apply gravity during wall slide
        if (stateMachine.currentState == wallSlide)
            return;

        if (grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = _stats.GroundingForce;
        }
        else
        {
            var inAirGravity = _stats.FallAcceleration; // Only 5f because 3.5x gravity scale multiplies it

            if (_endedJumpEarly && _frameVelocity.y > 0)
            {
                inAirGravity *= _stats.JumpEndEarlyGravityModifier;
            }

            // Reduce gravity significantly during the jump to allow horizontal distance
            if (_frameVelocity.y > -2f && _frameVelocity.y < 6f)
            {
                // Very low gravity during most of the jump for distance
                inAirGravity *= 0.1f; // 0.5f effective gravity (5f * 0.1f * 3.5x = 1.75f)
            }

            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
        }
    }


    private void HandleCustomDirection()
    {
        // Don't apply horizontal movement during wall slide
        if (stateMachine.currentState == wallSlide)
            return;

        // Air movement
        if (stateMachine.currentState == jumpState || stateMachine.currentState == airState)
        {
            if (_frameInput.Move.x != 0)
            {

                float currentDirection = Mathf.Sign(_frameVelocity.x);
                float inputDirection = Mathf.Sign(_frameInput.Move.x);

                if (currentDirection == inputDirection || Mathf.Abs(_frameVelocity.x) < _stats.MaxSpeed * 1.5f)
                {
                    float airAcceleration = _stats.Acceleration * 0.3f;
                    float targetSpeed = _frameInput.Move.x * _stats.MaxSpeed * 2.2f;
                    _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, targetSpeed, airAcceleration * Time.fixedDeltaTime);
                }
            }
            return;
        }

        // Ground movement - keep wall stopping for ground
        if (_frameInput.Move.x == 0)
        {
            var deceleration = grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Keep wall stopping for ground movement
            if (IsWallDetected() && Mathf.Sign(_frameInput.Move.x) == facingDirection)
            {
                _frameVelocity.x = 0f; // Stop movement when hitting wall on ground
                return;
            }

            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
        }
    }


    private void CornerCorrection()
    {
        // Enhanced corner correction for Unity 6
        if (Mathf.Abs(_frameVelocity.x) > 0.1f && grounded)
        {
            // Check for corner obstruction
            Vector2 rayOrigin = new Vector2(
                col.bounds.center.x + (col.bounds.extents.x * 0.8f * Mathf.Sign(_frameVelocity.x)),
                col.bounds.center.y - col.bounds.extents.y + 0.1f
            );

            RaycastHit2D cornerHit = Physics2D.Raycast(rayOrigin, Vector2.right * Mathf.Sign(_frameVelocity.x), 0.3f, whatIsGround);

            if (cornerHit.collider != null)
            {
                // Check if there's space above to push the player up
                Vector2 upRayOrigin = new Vector2(col.bounds.center.x, col.bounds.center.y + col.bounds.extents.y);
                RaycastHit2D upHit = Physics2D.Raycast(upRayOrigin, Vector2.up, 0.4f, whatIsGround);

                if (upHit.collider == null)
                {
                    // Push player up to clear the corner
                    _frameVelocity.y = Mathf.Max(_frameVelocity.y, 3f);
                    Debug.Log("Corner correction applied");
                }
            }
        }
    }

    // ...existing code...


    private void ApplyCustomMovement()
    {
        // Apply corner correction before setting velocity
        CornerCorrection();

        // Unity 6: Use velocity property (linearVelocity is deprecated)
        rb.linearVelocity = _frameVelocity;
        FlipController(_frameVelocity.x);
    }
    #endregion


    // makes some delay for the player to be able to attack again
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
    // ...existing code...

    public void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (GetDashPressed() && canDash && currentDashes > 0)
        {
            canDash = false;
            currentDashes--;

            // Get the current move input
            Vector2 moveInput = GetMoveInput();

            // If player is not pressing any direction, dash in facing direction
            if (moveInput.x == 0 && moveInput.y == 0)
            {
                dashDirection = facingDirection; // Dash horizontally in facing direction
                dashDirectionY = 0f; // No vertical component
            }
            else
            {
                // Use both X and Y input for full directional dashing
                dashDirection = moveInput.x; // Horizontal direction
                dashDirectionY = moveInput.y; // Vertical direction
            }

            stateMachine.ChangeState(dashState);
        }
    }

    // Add this new property for vertical dash direction
    public float dashDirectionY { get; private set; }

    // ...existing code...

    #region Input
    public Vector2 GetMoveInput()
    {
        return inputHandler.MoveInput;
    }

    public bool GetJumpPressed()
    {
        return inputHandler.JumpPressed;
    }

    public bool GetDashPressed()
    {
        return inputHandler.DashPressed;
    }
    #endregion

    #region Collision
    private void CheckIfGrounded()
    {
        if (isGrounded())
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }
    }

    public bool isGrounded()
    {
        bool boxColliderGrounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, whatIsGround).Length > 0;
        return grounded || boxColliderGrounded;
    }

    public bool IsWallDetected()
    {
        // Multiple point wall detection for better corner handling
        Vector3[] checkPoints = {
        wallCheck.position + Vector3.up * (col.size.y * 0.4f),
        wallCheck.position,
        wallCheck.position + Vector3.down * (col.size.y * 0.4f)
    };

        foreach (Vector3 point in checkPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
            if (hit.collider != null)
            {
                return true;
            }
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;

            if (Application.isPlaying)
            {
                Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * facingDirection * wallCheckDistance);
            }
            else
            {
                Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
                Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.left * wallCheckDistance);
            }
        }
    }
    #endregion

    #region Velocity
    public void ZeroVelocity()
    {
        _frameVelocity = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        _frameVelocity = new Vector2(_xVelocity, _yVelocity);
        rb.linearVelocity = _frameVelocity;
        FlipController(_xVelocity);
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

    public void Die()
    {

    }
}

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector2 Move;
}