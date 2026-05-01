using Robust.Shared.GameStates;

namespace Content.Shared._Scav.Movement;

/// <summary>
/// Does being above the floor count as touching a wall?
/// Restores the old style of zero-g movement for this entity, without being as extensive as MovementAlwaysTouchingComponent
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class MovementFloorCountsAsTouchingComponent : Component
{

}
