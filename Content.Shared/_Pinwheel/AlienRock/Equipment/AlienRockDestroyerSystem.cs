using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.Placeable;
using Content.Shared.Power.EntitySystems;
using System.Diagnostics.CodeAnalysis;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Manages linking and controlling the console & destroyer, and destruction
/// </summary>
public abstract partial class SharedAlienRockDestroyerSystem : EntitySystem
{
    [Dependency] private SharedPowerReceiverSystem _powerReceiver = default!;
    [Dependency] private SharedDeviceLinkSystem _deviceLink = default!;

    [SubscribeLocalEvent]
    private void OnItemPlaced(Entity<AlienRockDestroyerComponent> ent, ref ItemPlacedEvent args)
    {
        ent.Comp.CurrentArtifact = args.OtherEntity;
        Dirty(ent);
    }

    [SubscribeLocalEvent]
    private void OnItemRemoved(Entity<AlienRockDestroyerComponent> ent, ref ItemRemovedEvent args)
    {
        if (args.OtherEntity != ent.Comp.CurrentArtifact)
            return;

        ent.Comp.CurrentArtifact = null;
        Dirty(ent);
    }

    [SubscribeLocalEvent]
    private void OnConsoleMapInit(Entity<AlienRockConsoleComponent> ent, ref MapInitEvent args)
    {
        if (!TryComp<DeviceLinkSourceComponent>(ent, out var source))
            return;

        var linkedEntities = _deviceLink.GetLinkedSinks((ent.Owner, source), ent.Comp.LinkingPort);

        foreach (var sink in linkedEntities)
        {
            if (!TryComp<AlienRockDestroyerComponent>(sink, out var destroyer))
                continue;

            ent.Comp.Destroyer = sink;
            destroyer.Console = ent.Owner;
            Dirty(ent);
            Dirty(sink, destroyer);
            break;
        }
    }

    [SubscribeLocalEvent]
    private void OnNewLinkConsole(Entity<AlienRockConsoleComponent> ent, ref NewLinkEvent args)
    {
        if (args.SourcePort != ent.Comp.LinkingPort || !HasComp<AlienRockDestroyerComponent>(args.Sink))
            return;

        ent.Comp.Destroyer = args.Sink;
        Dirty(ent);
    }

    [SubscribeLocalEvent]
    private void OnNewLinkDestroyer(Entity<AlienRockDestroyerComponent> ent, ref NewLinkEvent args)
    {
        if (args.SinkPort != ent.Comp.LinkingPort || !HasComp<AlienRockConsoleComponent>(args.Source))
            return;

        ent.Comp.Console = args.Source;
        Dirty(ent);
    }

    [SubscribeLocalEvent]
    private void OnLinkAttemptConsole(Entity<AlienRockConsoleComponent> ent, ref LinkAttemptEvent args)
    {
        if (ent.Comp.Destroyer != null)
            args.Cancel(); // can only link to one device at a time
    }

    [SubscribeLocalEvent]
    private void OnLinkAttemptDestroyer(Entity<AlienRockDestroyerComponent> ent, ref LinkAttemptEvent args)
    {
        if (ent.Comp.Console != null)
            args.Cancel(); // can only link to one device at a time
    }

    [SubscribeLocalEvent]
    private void OnPortDisconnectedConsole(Entity<AlienRockConsoleComponent> ent, ref PortDisconnectedEvent args)
    {
        if (args.Port != ent.Comp.LinkingPort || ent.Comp.Destroyer == null)
            return;

        ent.Comp.Destroyer = null;
        Dirty(ent);
    }

    [SubscribeLocalEvent]
    private void OnPortDisconnectedDestroyer(Entity<AlienRockDestroyerComponent> ent, ref PortDisconnectedEvent args)
    {
        if (args.Port != ent.Comp.LinkingPort || ent.Comp.Console == null)
            return;

        ent.Comp.Console = null;
        Dirty(ent);
    }

    public bool TryGetDestroyer(Entity<AlienRockConsoleComponent> ent,
    [NotNullWhen(true)] out Entity<AlienRockDestroyerComponent>? destroyer)
    {
        destroyer = null;

        var consoleEnt = ent.Owner;
        if (!_powerReceiver.IsPowered(consoleEnt))
            return false;

        if (!TryComp<AlienRockDestroyerComponent>(ent.Comp.Destroyer, out var destroyerComp))
            return false;

        if (!_powerReceiver.IsPowered(ent.Comp.Destroyer.Value))
            return false;

        destroyer = (ent.Comp.Destroyer.Value, destroyerComp);
        return true;
    }

    public bool TryGetArtifactFromConsole(Entity<AlienRockConsoleComponent> ent,
        [NotNullWhen(true)] out Entity<AlienRockComponent>? artifact)
    {
        artifact = null;

        if (!TryGetDestroyer(ent, out var destroyer))
            return false;

        if (!TryComp<AlienRockComponent>(destroyer.Value.Comp.CurrentArtifact, out var comp))
            return false;

        artifact = (destroyer.Value.Comp.CurrentArtifact.Value, comp);
        return true;
    }

    public bool TryGetConsole(Entity<AlienRockDestroyerComponent> ent,
        [NotNullWhen(true)] out Entity<AlienRockConsoleComponent>? console)
    {
        console = null;

        if (!TryComp<AlienRockConsoleComponent>(ent.Comp.Console, out var consoleComp))
            return false;

        console = (ent.Comp.Console.Value, consoleComp);
        return true;
    }
}
