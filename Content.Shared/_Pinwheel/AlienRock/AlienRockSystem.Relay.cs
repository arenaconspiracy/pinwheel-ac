using Content.Shared.Examine;
using Content.Shared.Interaction;

namespace Content.Shared._Pinwheel.AlienRock;

public sealed partial class AlienRockSystem
{
    private void InitializeRelay()
    {
        SubscribeLocalEvent<AlienRockComponent, ExaminedEvent>(RelayEvent);
        SubscribeLocalEvent<AlienRockComponent, InteractUsingEvent>(RelayEvent);
    }

    private void RelayEvent<T>(Entity<AlienRockComponent> ent, ref T args) where T : EntityEventArgs
    {
        CoreRelayEvent(ent, ref args);
    }

    private void RefRelayEvent<T>(Entity<AlienRockComponent> ent, ref T args)
    {
        var ev = CoreRelayEvent(ent, ref args);
        args = ev.Args;
    }

    private AlienRockRelayedEvent<T> CoreRelayEvent<T>(Entity<AlienRockComponent> ent, ref T args)
    {
        var ev = new AlienRockRelayedEvent<T>(args);

        // fetching it like this because ent.Comp.Nodes doesn't work w/ foreach
        _container.TryGetContainer(ent.Owner, AlienRockComponent.ContainerId, out var nodes);

        foreach (var node in nodes!.ContainedEntities)
        {
            Log.Info($"{node}");
            RaiseLocalEvent(node, ref ev);
        }

        return ev;
    }
}

[ByRefEvent]
public sealed class AlienRockRelayedEvent<TEvent> : EntityEventArgs
{
    public TEvent Args;

    public AlienRockRelayedEvent(TEvent args)
    {
        Args = args;
    }
}
