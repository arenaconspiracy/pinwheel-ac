using Content.Shared.DeviceLinking;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Console used for abating artifacts
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class AlienRockConsoleComponent : Component
{
    /// <summary>
    /// The destroyer we are linked to, if any
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Destroyer;

    /// <summary>
    /// The machine linking port for linking the console with the analyzer.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> LinkingPort = "ArtifactDestroyerSource";
}

/// <summary>
/// Machine used for abating artifacts
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class AlienRockDestroyerComponent : Component
{
    /// <summary>
    /// The current artifact placed on this analyzer, if any
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? CurrentArtifact;

    /// <summary>
    /// The console we are linked to, if any
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? Console;

    /// <summary>
    /// Sound to play when destroying artifacts
    /// </summary>
    [DataField]
    public SoundSpecifier? DestroySound = new SoundCollectionSpecifier();

    /// <summary>
    /// Effect to spawn in place of an artifact when destroying them
    /// </summary>
    [DataField]
    public EntProtoId DestroyEffect = "EffectEmpPulse";

    /// <summary>
    /// The machine linking port for linking the analyzer with the console
    /// </summary>
    [DataField]
    public ProtoId<SinkPortPrototype> LinkingPort = "ArtifactDestroyerSink";
}

[Serializable, NetSerializable]
public enum AlienRockConsoleUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class AlienRockConsoleButtonPressedMessage : BoundUserInterfaceMessage;
