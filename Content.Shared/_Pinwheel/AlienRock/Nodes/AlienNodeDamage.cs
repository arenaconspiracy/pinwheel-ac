using Content.Shared.Damage.Systems;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.AlienRock;

[RegisterComponent]
public sealed partial class AlienNodeDamageComponent : Component
{
    [DataField]
    public FixedPoint2 DamageThreshold = 90;
}

public sealed partial class AlienNodeDamageSystem : AlienNodeBaseSystem
{
    [Dependency] private DamageableSystem _damage = default!;

    [SubscribeLocalEvent]
    private void OnDamageChanged(Entity<AlienNodeDamageComponent> node,
        ref AlienRockRelayedEvent<DamageDealtEvent> rel)
    {
        if (_damage.GetTotalDamage(rel.Artifact) >= node.Comp.DamageThreshold)
            NodeRemove(node.Owner);
    }
}
