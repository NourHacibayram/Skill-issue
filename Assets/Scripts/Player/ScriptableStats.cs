using UnityEngine;

[CreateAssetMenu]
public class ScriptableStats : ScriptableObject
{
    [Header("LAYERS")]
    [Tooltip("Set this to the layer your player is on")]
    public LayerMask PlayerLayer;

    [Header("INPUT")]
    [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
    public bool SnapInput = true;

    [Tooltip("Minimum input required before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
    public float VerticalDeadZoneThreshold = 0.3f;

    [Tooltip("Minimum input required before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
    public float HorizontalDeadZoneThreshold = 0.1f;

    [Header("MOVEMENT")]
    [Tooltip("The top horizontal movement speed")]
    public float MaxSpeed = 6f;

    [Tooltip("The player's capacity to gain horizontal speed")]
    public float Acceleration = 50f;

    [Tooltip("The pace at which the player comes to a stop")]
    public float GroundDeceleration = 40f;

    [Tooltip("Deceleration in air only after stopping input mid-air")]
    public float AirDeceleration = 15f;

    [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
    public float GroundingForce = -1.5f;

    [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
    public float GrounderDistance = 0.05f;

    [Header("JUMP")]
    [Tooltip("The immediate velocity applied when jumping")]
    public float JumpPower = 8;

    [Tooltip("The maximum vertical movement speed")]
    public float MaxFallSpeed = 16;

    [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
    public float FallAcceleration = 5;

    [Tooltip("The gravity multiplier added when jump is released early")]
    public float JumpEndEarlyGravityModifier = 1.2f;

    [Tooltip("The time before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
    public float CoyoteTime = .15f; [Tooltip("The amount of time we buffer a jump. This allows jump input before actually hitting the ground")]
    public float JumpBuffer = .2f; [Header("WALL SLIDE")]
    [Tooltip("The downward speed when sliding on a wall")]
    public float WallSlideSpeed = 1.4f;

    [Header("WALL JUMP")]
    [Tooltip("The horizontal force applied when wall jumping")]
    public float WallJumpForceX = 8f;

    [Tooltip("The vertical force applied when wall jumping")]
    public float WallJumpForceY = 12f;
}