using Content.Shared._Scav.Movement;
using Content.Shared.Gravity;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;

namespace Content.Shared.Movement.Systems;

public sealed class MovementIgnoreGravitySystem : EntitySystem
{
    [Dependency] SharedGravitySystem _gravity = default!;

    private EntityQuery<TransformComponent> _xformQuery;

    public override void Initialize()
    {
        SubscribeLocalEvent<MovementAlwaysTouchingComponent, CanWeightlessMoveEvent>(OnWeightless);
        SubscribeLocalEvent<MovementFloorCountsAsTouchingComponent, CanWeightlessMoveEvent>(OnFloorWeightless);
        SubscribeLocalEvent<MovementIgnoreGravityComponent, IsWeightlessEvent>(OnIsWeightless);
        SubscribeLocalEvent<MovementIgnoreGravityComponent, ComponentStartup>(OnComponentStartup);

        _xformQuery = GetEntityQuery<TransformComponent>();
    }

    // Scav: Added handler for MovementFloorCountsAsTouchingComponent
    private void OnFloorWeightless(Entity<MovementFloorCountsAsTouchingComponent> entity, ref CanWeightlessMoveEvent args)
    {
        if (_xformQuery.TryGetComponent(entity.Owner, out var xform) && xform.GridUid != null)
            args.CanMove = true;
    }
    // End Scav

    private void OnWeightless(Entity<MovementAlwaysTouchingComponent> entity, ref CanWeightlessMoveEvent args)
    {
        args.CanMove = true;
    }

    private void OnIsWeightless(Entity<MovementIgnoreGravityComponent> entity, ref IsWeightlessEvent args)
    {
        // We don't check if the event has been handled as this component takes precedent over other things.

        args.IsWeightless = entity.Comp.Weightless;
        args.Handled = true;
    }

    private void OnComponentStartup(Entity<MovementIgnoreGravityComponent> entity, ref ComponentStartup args)
    {
        EnsureComp<GravityAffectedComponent>(entity);
        _gravity.RefreshWeightless(entity.Owner, entity.Comp.Weightless);
    }
}
