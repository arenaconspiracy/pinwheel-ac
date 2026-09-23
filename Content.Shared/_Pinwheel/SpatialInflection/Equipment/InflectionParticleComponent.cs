using Content.Shared.Tag;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.Locus;

/// <summary>
/// Particle projectile used to abate spatial inflection loci <see cref="InflectionLocusComponent"/>
/// </summary>
[RegisterComponent]
public sealed partial class InflectionParticleComponent : Component
{
    /// <summary>
    /// Type of this particle
    /// </summary>
    /// <remarks>
    /// This is using tags for their validation
    /// <see cref="TagComponent"/> can DIAF but arbitrarily defined validated bits of data are really good
    /// </remarks>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public ProtoId<TagPrototype> ParticleType = "ParticleDelta";
}
