# Wall Climb Feature Implementation

## Overview
Added a new PlayerWallClimbState that allows the player to climb walls using the Unity Input System with dedicated WallClimb and ClimbMovement actions.

## Files Modified/Created

### 1. PlayerInputHandler.cs
- Added `WallClimbPressed` property to track wall climb action
- Added `WallClimbMovementInput` property for climbing movement
- Integrated with Unity Input System actions: `WallClimb` and `ClimbMovement`
- Updated `ResetInputs()` method to include wall climb inputs

### 2. PlayerWallClimbState.cs (NEW FILE)
- Active climbing state that handles vertical movement on walls
- Uses Input System for both activation and movement
- Transitions to WallClimbIdleState when no vertical input is given
- Automatically exits to appropriate states when conditions change

### 3. PlayerWallClimbIdleState.cs (NEW FILE)
- Idle climbing state for hanging on walls without moving
- Player stays attached to wall with zero velocity
- Transitions to PlayerWallClimbState when vertical input is detected
- Provides better control separation between active climbing and hanging

### 3. Player.cs
- Added `wallClimbState` property for active climbing
- Added `wallClimbIdleState` property for hanging on walls
- Added `GetWallClimbPressed()` input method
- Added `GetWallClimbHeld()` method for continuous climbing detection
- Initialized both wall climb states in Awake()

### 4. PlayerAirState.cs
- Added transition to wall climb idle state when WallClimb action is pressed while against a wall

### 5. PlayerWallSlideState.cs
- Added transition to wall climb idle state when WallClimb action is pressed

### 6. PlayerControls.cs (Auto-generated)
- Contains `WallClimb` action (bind to your preferred key, e.g., 'K')
- Contains `ClimbMovement` action (Vector2 input for climb direction)

## Input System Setup
The wall climb feature uses two Input System actions:

1. **WallClimb Action**: Button-type action for initiating wall climb
   - Bind this to your preferred key (e.g., 'K')
   - Type: Button
   - Used to enter wall climb state

2. **ClimbMovement Action**: Vector2-type action for climbing movement
   - Bind this to WASD, Arrow Keys, or gamepad stick
   - Type: Value (Vector2)
   - Used for vertical climbing movement during wall climb

## How to Use
1. **Approach a wall** - Player must be facing the wall and in air or wall slide state
2. **Press WallClimb action** - Activates wall climb mode
3. **Use ClimbMovement input** - Move up and down the wall
4. **Keep pressing towards wall** - Must maintain directional input to stay attached
5. **Release directional input** - Returns to wall slide state
6. **Press Jump** - Performs wall jump while climbing

## State Transitions
- **Air State → Wall Climb Idle**: When WallClimb action is pressed while against a wall
- **Wall Slide → Wall Climb Idle**: When WallClimb action is pressed
- **Wall Climb Idle → Wall Climb**: When vertical input is detected on ClimbMovement
- **Wall Climb → Wall Climb Idle**: When no vertical input is given
- **Wall Climb/Wall Climb Idle → Wall Jump**: When Jump action is pressed
- **Wall Climb/Wall Climb Idle → Wall Slide**: When directional input is released
- **Wall Climb/Wall Climb Idle → Air State**: When no wall detected or moving away from wall
- **Wall Climb/Wall Climb Idle → Idle**: When touching ground

## Climb Mechanics
- **Climb Speed**: 3 units/second upward
- **Descent Speed**: 1.5 units/second downward (50% of climb speed)
- **Wall Climb Idle**: Player hangs on wall with zero velocity when no vertical input
- **Active Climbing**: Player moves up/down when giving vertical input
- **State Separation**: Idle hanging and active climbing are separate states for better control
- **Wall Requirements**: Must continuously press towards wall to maintain attachment
- **Input Separation**: Wall climb activation and movement are separate inputs for better control

## Technical Notes
- Uses Unity Input System for all inputs (no hardcoded KeyCode dependencies)
- Uses Unity's `linearVelocity` property (Unity 6 compatible)
- Includes debug logging for state transitions
- Properly handles facing direction changes
- Graceful exits to appropriate states based on context
- Separate ClimbMovement input allows for dedicated climbing controls

## Configuration
To configure the wall climb feature:
1. Open the PlayerControls.inputactions file in Unity
2. Bind the WallClimb action to your desired key
3. Bind the ClimbMovement action to your movement controls
4. Regenerate the PlayerControls.cs class if needed
