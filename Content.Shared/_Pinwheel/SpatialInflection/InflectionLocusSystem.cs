using Content.Shared.Random.Helpers;
using Content.Shared.Tag;
using Robust.Shared.Timing;

namespace Content.Shared._Pinwheel.Locus;

/// <summary>
/// TBA
/// </summary>
public sealed partial class InflectionLocusSystem : EntitySystem
{
    [Dependency] private IGameTiming _timing = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<InflectionLocusComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if ((_timing.CurTime < comp.CriticalAt)
                || (comp.Critical))
                continue;

            GoCritical((uid, comp));
        }
    }

    private void GoCritical(Entity<InflectionLocusComponent> ent)
    { // could reasonably just be part of the query loop but whatever
        ent.Comp.Critical = true;

        var ev = new InflectionCriticalEvent();
        RaiseLocalEvent(ent.Owner, ref ev);
    }

    [SubscribeLocalEvent]
    private void OnMapInit(Entity<InflectionLocusComponent> ent, ref MapInitEvent args)
    {
        var rand = SharedRandomExtensions.PredictedRandom(
            _timing, GetNetEntity(ent)); // TODO: this is not fucking predicted whatsoever
            // so we're relying on networking to bulldoze the client values

        var rTime = rand.Next(ent.Comp.CriticalMin, ent.Comp.CriticalMax);
        ent.Comp.CriticalAt = _timing.CurTime + rTime; // TODO: this doesn't happen on clients for some reason

        for (int i = 0; i < ent.Comp.ParticleTotal; i++)
        {
            var r = rand.Next(ent.Comp.ParticleTypes.Count);
            ent.Comp.ParticleList.Add(ent.Comp.ParticleTypes[r]);
        }

        Dirty(ent); // we probably want to dirty this whether or not it's predicted? but i rather it be predicted
    }

    [SubscribeLocalEvent]
    private void OnCritical( // TMP
        Entity<InflectionLocusComponent> ent,
        ref InflectionCriticalEvent args)
    {
        Log.Info($"{ToPrettyString(ent)} has gone critical");
    }
}

/// <summary>
/// Raised on a locus when it goes supercritical, handled by individual effects
/// </summary>
[ByRefEvent]
public readonly record struct InflectionCriticalEvent();

/// <summary>
/// Raised on a locus when hit by an anomalous particle
/// </summary>
[ByRefEvent]
public readonly record struct InflectionAffectedEvent(EntityUid locus, ProtoId<TagPrototype> type);
