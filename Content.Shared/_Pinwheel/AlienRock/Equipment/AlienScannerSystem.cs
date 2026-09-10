using Content.Shared.Verbs;
using Content.Shared.Timing;
using Content.Shared.Interaction;
using Content.Shared._Pinwheel.AlienRock;
using Robust.Shared.Timing;

namespace Content.Shared._Pinwheel.AlienRock.Equipment;

/// <summary>
/// Handheld tool displaying the nodes present on an artifact
/// </summary>
public sealed partial class AlienScannerSystem : EntitySystem
{
    [Dependency] private IGameTiming _timing = default!;
    [Dependency] private SharedTransformSystem _transform = default!;
    [Dependency] private SharedUserInterfaceSystem _ui = default!;
    [Dependency] private UseDelaySystem _useDelay = default!;

    public override void Update(float frameTime)
    {
        var scannerQuery = EntityQueryEnumerator<
            AlienScannerComponent,
            AlienScannerConnectedComponent>();
        while (scannerQuery.MoveNext(out var uid, out var scan, out var con))
        {
            if (con.UpdateNext > _timing.CurTime)
                continue;

            con.UpdateNext = _timing.CurTime + con.UpdateRate;

            var xform1 = Transform(uid);
            var xform2 = Transform(con.Attached);
            if (!_transform.InRange(xform1.Coordinates, xform2.Coordinates, scan.Range))
            {
                //scanner is too far, disconnect
                RemCompDeferred(uid, con);
            }
        }
    }

    [SubscribeLocalEvent]
    private void OnBeforeRangedInteract(
        Entity<AlienScannerComponent> ent,
        ref BeforeRangedInteractEvent args)
    {
        if (args.Handled
            || !args.CanReach
            || args.Target is not { } target
            || !TryComp<AlienRockComponent>(target, out var rock))
            return;

        Attach(ent, (target, rock), args.User);

        args.Handled = true;
    }

    [SubscribeLocalEvent]
    private void AddScanVerb(
        Entity<AlienScannerComponent> ent,
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
        Entity<AlienScannerComponent> ent,
        Entity<AlienRockComponent> rock,
        EntityUid actor
    )
    {
        if (TryComp(ent, out UseDelayComponent? useDelay)
            && !_useDelay.TryResetDelay((ent, useDelay), true))
            return;

        var connected = EnsureComp<AlienScannerConnectedComponent>(ent);
        if (connected.Attached != rock.Owner)
        {
            connected.Attached = rock.Owner;
            Dirty(ent, connected);
        }

        _ui.TryOpenUi((ent, null), AlienScannerUiKey.Key, actor, predicted: true);
    }
}
