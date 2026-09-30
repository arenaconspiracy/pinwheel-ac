using Content.Shared.Tag;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Pinwheel.SpatialInflection;

/// <summary>
/// TBA
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(fieldDeltas: true)]
public sealed partial class InflectionLocusComponent : Component
{
    /// <summary>
    /// List of particle types that needs to be received in the right order to abate the inflection
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    [AutoNetworkedField]
    public List<ProtoId<TagPrototype>> ParticleList
        = new List<ProtoId<TagPrototype>>();

    /// <summary>
    /// List of possible particle types that can be added to <see cref="ParticleList">
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public List<ProtoId<TagPrototype>> ParticleTypes
        = new List<ProtoId<TagPrototype>> {
            "ParticleDelta",
            "ParticleEpsilon",
            "ParticleSigma",
            "ParticleZeta"
        };

    /// <summary>
    /// The amount of steps <see cref="ParticleList"> should have
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public int ParticleTotal = 6;

    /// <summary>
    /// Which step of the <see cref="ParticleList"> is the locus at
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    [AutoNetworkedField]
    public int ParticleComplete = 0;

    /// <summary>
    /// When should the locus explode
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan CriticalAt = TimeSpan.Zero;

    /// <summary>
    /// Maximum time after spawn to blow up at
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan CriticalMax = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Minimum time after spawn to blow up at
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan CriticalMin = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Is this locus currently going critical
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public LocusState State = LocusState.Live;

    /// <summary>
    /// Sound played at mapinit
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public SoundSpecifier? SoundSpawn;
}

[Serializable, NetSerializable]
public enum LocusState : byte
{
    Live,
    Abated,
    Critical,
}
