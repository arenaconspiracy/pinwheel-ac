using Content.Shared.Examine;
using Content.Shared.Interaction;

namespace Content.Shared._Pinwheel.AlienRock;

public sealed partial class AlienNodeDebugSystem : AlienNodeBaseSystem
{
    [SubscribeLocalEvent]
    private void OnExamine(Entity<AlienNodeDebugComponent> ent,
        ref AlienRockRelayedEvent<ExaminedEvent> args)
    {
        Log.Info("Node examined");
        NodeRemove(ent.Owner);
    }

    [SubscribeLocalEvent]
    private void OnInteractUsing(Entity<AlienNodeDebugComponent> ent,
        ref AlienRockRelayedEvent<InteractUsingEvent> args)
    {
        Log.Info("Node interacted using");
        NodeRemove(ent.Owner);
    }
}
