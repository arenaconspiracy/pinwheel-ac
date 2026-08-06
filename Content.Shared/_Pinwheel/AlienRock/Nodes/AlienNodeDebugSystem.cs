using Content.Shared.Examine;
using Content.Shared.Interaction;

namespace Content.Shared._Pinwheel.AlienRock;

public sealed partial class AlienNodeDebugSystem : EntitySystem
{
    [SubscribeLocalEvent]
    private void OnExamine(Entity<AlienNodeDebugComponent> ent,
        ref AlienRockRelayedEvent<ExaminedEvent> args)
    {
        Log.Info("Node examined");
    }

    [SubscribeLocalEvent]
    private void OnInteractUsing(Entity<AlienNodeDebugComponent> ent,
        ref AlienRockRelayedEvent<InteractUsingEvent> args)
    {
        Log.Info("Node interacted using");
    }
}
