using Content.Shared.Radiation.Components;
using Content.Shared.Radiation.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.AlienRock;

[RegisterComponent]
public sealed partial class AlienEffectRadiationComponent : Component
{}

public sealed partial class AlienEffectRadiationSystem : AlienNodeBaseSystem
{
    [Dependency] private SharedContainerSystem _container = default!;
    [Dependency] private SharedRadiationSystem _radiation = default!;

    private void AdjustRadiation(Entity<AlienEffectRadiationComponent> ent)
    {
        if (!_container.TryGetContainer(ent.Owner, AlienRockComponent.ContainerId, out var nodes))
            return;

        if (!TryComp(ent.Owner, out RadiationSourceComponent? radiation))
        {
            Log.Error($"{ToPrettyString(ent)} has no RadiationEmitterComponent");
            return;
        }

        // BAD: magic numbers city
        // WORSE: DIFFERENT MAGIC NUMBERS FROM FUCKING AlienRockSystem
        var intensity = (1 + (nodes.Count / 1.5));

        _radiation.SetIntensity((ent.Owner, radiation), (float)intensity);

    }

    [SubscribeLocalEvent]
    private void OnEntRemovedFromContainer(Entity<AlienEffectRadiationComponent> ent,
        ref EntRemovedFromContainerMessage args)
    {
        AdjustRadiation(ent);
    }

    [SubscribeLocalEvent]
    private void OnMapInit(Entity<AlienEffectRadiationComponent> ent,
        ref MapInitEvent args)
    {
        AdjustRadiation(ent);
    }
}
