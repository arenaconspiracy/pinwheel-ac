using Content.Shared.Random.Helpers;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Serialization;

namespace Content.Shared._Pinwheel.SpatialInflection;

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
                || (comp.State != LocusState.Live))
                continue;

            GoCritical((uid, comp));
        }
    }

    private void GoCritical(Entity<InflectionLocusComponent> ent)
    { // could reasonably just be part of the query loop but whatever
        ent.Comp.State = LocusState.Critical;

        var ev = new InflectionCriticalEvent();
        RaiseLocalEvent(ent.Owner, ref ev);
    }

    private void ParticleCheck(
        Entity<InflectionLocusComponent> ent,
        ProtoId<TagPrototype> type)
    {
        if (!ent.Comp.ParticleTypes.Contains(type))
        {
            Log.Warning($"{ToPrettyString(ent)} hit with inflection particle of invalid type.");
            return;
        }

        if (ent.Comp.ParticleList[ent.Comp.ParticleComplete] == type)
            ent.Comp.ParticleComplete++; // look ma i'm a real programmer
        else
            ent.Comp.ParticleComplete = 0;

        DirtyField(ent.AsNullable(), nameof(ent.Comp.ParticleComplete));

        if (ent.Comp.ParticleComplete == ent.Comp.ParticleTotal)
        {
            var ev = new InflectionAbatedEvent();
            RaiseLocalEvent(ent, ref ev);
            return;
        }
    }

    [SubscribeLocalEvent]
    private void OnMapInit(
        Entity<InflectionLocusComponent> ent,
        ref MapInitEvent args)
    { // this whole method is fucked
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
    private void OnAffected(
        Entity<InflectionLocusComponent> ent,
        ref InflectionAffectedEvent args)
    {
        if (ent.Comp.State != LocusState.Live)
            return;

        ParticleCheck(ent, args.Type);
    }

    [SubscribeLocalEvent]
    private void OnAbated(
        Entity<InflectionLocusComponent> ent,
        ref InflectionAbatedEvent args)
    { // TODO: this is never predicted, see AffectedEvent remarks
        ent.Comp.State = LocusState.Abated;
        Log.Info($"{ToPrettyString(ent)} has been abated");
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
/// Raised on a locus when abated by particles
/// </summary>
[ByRefEvent]
public readonly record struct InflectionAbatedEvent();

/// <summary>
/// Raised on a locus when hit by an anomalous particle
/// </summary>
/// <remarks>
/// only raised on server because it's handled by the projectile
/// TODO: figure out a way to handle it elsewhere/predictively
/// </remarks>
[ByRefEvent]
public readonly record struct InflectionAffectedEvent(ProtoId<TagPrototype> Type);

/// <summary>
/// Raised on a locus when it goes supercritical, handled by individual effects
/// </summary>
[ByRefEvent]
public readonly record struct InflectionCriticalEvent();
