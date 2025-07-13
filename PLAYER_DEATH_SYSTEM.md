# Player Death System Implementation

## Overview
The player death system has been implemented with two different death types:
1. **Spike Death** - Triggers "ShockLight" animation
2. **Void Death** - Player falls and gets thrown back upon respawn

## Components Added

### 1. PlayerDeadState.cs
- Handles player death logic
- Manages respawn after delay
- Plays appropriate animations based on death type
- Respawns player at level spawn point

### 2. Hazard Scripts

#### SpikeHazard.cs
- Attach to spike objects
- Requires trigger collider
- Kills player with "Spike" death type
- Plays shock animation

#### VoidZone.cs  
- Can be used as trigger zone or Y-threshold monitor
- Kills player with "Void" death type
- Good for bottomless pits

#### DeathZone.cs
- General purpose death trigger
- Configurable death type
- Simple to set up

## Setup Instructions

### For Spike Hazards:
1. Create spike GameObject
2. Add SpikeHazard script
3. Add trigger Collider2D
4. Optional: Add spike hit sound and visual effects

### For Void Zones:
1. Create empty GameObject at bottom of level
2. Add VoidZone script
3. Add large trigger Collider2D covering bottom area
4. Optional: Set Y threshold for automatic detection

### For General Death Zones:
1. Create empty GameObject
2. Add DeathZone script
3. Add trigger Collider2D
4. Set death type in inspector

### Player Setup:
1. Player class automatically initializes dead state
2. Add death sound clip in Player inspector
3. Ensure Player has "Player" tag

### Level Setup:
1. Ensure Level script has playerSpawnPoint set
2. Spawn point should be positioned safely above ground

## Animation Requirements

The system expects these animation triggers:
- **"ShockLight"** - For spike deaths (electrocution effect)
- **"Death"** - For void deaths (falling animation)
- **"Dead"** - Animation bool for dead state

## Audio Setup

Add these audio clips to Player:
- **jumpSound** - Existing jump sound
- **fallSound** - Existing fall sound  
- **deathSound** - New death sound

Add these to hazard objects:
- **spikeHitSound** - Sound when hit by spikes
- **voidFallSound** - Sound when falling into void

## Features

### Respawn System:
- 2 second death delay
- Automatic respawn at level spawn point
- Void deaths add slight upward velocity (thrown effect)
- Spike deaths respawn with zero velocity

### Death Prevention:
- Player becomes "busy" during death to prevent input
- Death state prevents multiple death triggers
- Safe respawning at designated spawn points

### Audio/Visual Feedback:
- Different animations for different death types
- Audio feedback for deaths and hazards
- Optional visual effects for spikes

## Usage Examples

```csharp
// Manually trigger player death from code:
player.deadState.SetDeathType(PlayerDeadState.DeathType.Spike);
player.stateMachine.ChangeState(player.deadState);

// Check if player is dead:
if (player.stateMachine.currentState == player.deadState)
{
    // Player is currently dead
}
```

## Notes
- System integrates with existing state machine
- Compatible with all existing player states
- Respawn uses existing Level spawn point system
- Death system preserves player facing direction
- Works with Unity's 2D physics and collision system
