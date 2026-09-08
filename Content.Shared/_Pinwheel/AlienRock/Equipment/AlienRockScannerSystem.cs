using Content.Shared.Verbs;
using Content.Shared.Timing;
using Content.Shared.Interaction;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Handheld tool displaying the nodes present on an artifact
/// </summary>
public sealed partial class AlienRockScannerSystem : EntitySystem
{
    [Dependency] private SharedUserInterfaceSystem _ui = default!;
    [Dependency] private UseDelaySystem _useDelay = default!;

    [SubscribeLocalEvent]
    private void OnBeforeRangedInteract(
        Entity<AlienRockScannerComponent> ent,
        ref BeforeRangedInteractEvent args)
    {
        if (args.Handled
            || !args.CanReach
            || args.Target is not { } target
            || !HasComp<AlienRockComponent>(target))
            return;

        args.Handled = true;
    }

    [SubscribeLocalEvent]
    private void AddScanVerb(
        Entity<AlienRockScannerComponent> ent,
        ref GetVerbsEvent<UtilityVerb> args)
    {
        if (!args.CanAccess)
            return;

        if (!TryComp<AlienRockComponent>(args.Target, out var rock))
            return;

        var target = new EntityUid((int)args.Target); // can't pass ref to Attach() below otherwise
        var actor = new EntityUid((int)args.User);

        var verb = new UtilityVerb
        {
            Act = () => Attach((ent), (target, rock), actor),
            Text = Loc.GetString("node-scan-tooltip")
        };

        args.Verbs.Add(verb);
    }

    private void Attach(
        Entity<AlienRockScannerComponent> ent,
        Entity<AlienRockComponent> rock,
        EntityUid actor
    )
    {
        if (TryComp(ent, out UseDelayComponent? useDelay)
            && !_useDelay.TryResetDelay((ent, useDelay), true))
            return;

        var connected = EnsureComp<AlienRockScannerConnectedComponent>(ent);
        if (connected.AttachedTo != rock.Owner)
        {
            connected.AttachedTo = rock.Owner;
            Dirty(ent, connected);
        }

        _ui.TryOpenUi((ent, null), AlienRockScannerUiKey.Key, actor, predicted: true);
    }
}
