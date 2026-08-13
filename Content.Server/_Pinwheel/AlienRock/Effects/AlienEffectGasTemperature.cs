using Content.Server.Atmos.EntitySystems;
using Content.Shared._Pinwheel.AlienRock;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Server._Pinwheel.AlienRock;

[RegisterComponent]
public sealed partial class AlienEffectGasTemperatureComponent : Component
{
    /// <summary>
    /// Whether temperature is meant to go up. False means it goes Down
    /// </summary>
    [DataField]
    public bool Heating = true;

    /// <summary>
    /// Strength to adjust the tile temperature by per second. I have no idea what unit this is in
    /// </summary>
    [DataField]
    public float Strength = 20;
}

public sealed partial class AlienEffectGasTemperatureSystem : AlienNodeBaseSystem
{
    [Dependency] private AtmosphereSystem _atmosphere = default!;
    [Dependency] private SharedContainerSystem _container = default!;
    [Dependency] private SharedTransformSystem _xform = default!;

    private void AdjustStrength(Entity<AlienEffectGasTemperatureComponent> ent)
    {
        if (!_container.TryGetContainer(ent.Owner, AlienRockComponent.ContainerId, out var nodes))
            return;

        // BAD: MAGIC NUMBER
        var strength = (nodes.Count * 3);

        ent.Comp.Strength = strength;
    }

    [SubscribeLocalEvent]
    private void OnEntRemovedFromContainer(Entity<AlienEffectGasTemperatureComponent> ent,
        ref EntRemovedFromContainerMessage args)
    {
        AdjustStrength(ent);
    }

    [SubscribeLocalEvent]
    private void OnMapInit(Entity<AlienEffectGasTemperatureComponent> ent,
        ref MapInitEvent args)
    {
        AdjustStrength(ent);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<AlienEffectGasTemperatureComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var gas, out var xform))
        {
            var grid = xform.GridUid;
            var map = xform.MapUid;
            var sign = gas.Heating ? 1 : -1 ;
            var indices = _xform.GetGridTilePositionOrDefault((uid, xform));
            var mixture = _atmosphere.GetTileMixture(grid, map, indices, true);

            if (mixture is { })
                mixture.Temperature += (sign * gas.Strength) * frameTime;
        }
    }
}
