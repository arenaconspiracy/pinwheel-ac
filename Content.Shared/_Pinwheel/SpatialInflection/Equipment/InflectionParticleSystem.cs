using Content.Shared.Projectiles;

namespace Content.Shared._Pinwheel.SpatialInflection;
/// <summary>
/// Handles <see cref="BeforeProjectileHitEvent"/>
/// to raise a <see cref="InflectionAffectedEvent"/> on <see cref="InflectionLocusComponent"/>
/// </summary>
/// <remarks>
/// As of writing <see cref="BeforeProjectileHitEvent"> is only raised serverside, but is defined in shared.
/// This is done this way because there's not a convenient way to handle this in <see cref="InflectionLocusSystem">
/// besides physics events which I want to avoid
/// </remarks>
public sealed partial class InflectionParticleSystem : EntitySystem
{
    [Dependency] private EntityQuery<InflectionLocusComponent> _loci = default!;

    [SubscribeLocalEvent]
    private void OnBeforeProjectileHit(
        Entity<InflectionParticleComponent> ent,
        ref BeforeProjectileHitEvent args)
    {
        InflectionLocusComponent? locus = null;

        if (!_loci.Resolve(args.Target, ref locus, false))
            return;

        var ev = new InflectionAffectedEvent(ent.Comp.ParticleType);
        RaiseLocalEvent(args.Target, ref ev);
    }
}
