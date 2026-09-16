using Content.Shared.Damage.Systems;
using Content.Shared.FixedPoint;
using Content.Shared._Pinwheel.AlienRock;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.AlienRock.Nodes;

[RegisterComponent]
public sealed partial class AlienNodeDamageComponent : Component
{
    [DataField]
    public FixedPoint2 DamageThreshold = 90;
}

public sealed partial class AlienNodeDamageSystem : EntitySystem
{
    [Dependency] private DamageableSystem _damage = default!;

    [SubscribeLocalEvent]
    private void OnDamageChanged(Entity<AlienNodeDamageComponent> node,
        ref AlienRockRelayedEvent<DamageDealtEvent> rel)
    {
        if (_damage.GetTotalDamage(rel.Artifact) >= node.Comp.DamageThreshold)
            PredictedQueueDel(node.Owner);
    }
}
