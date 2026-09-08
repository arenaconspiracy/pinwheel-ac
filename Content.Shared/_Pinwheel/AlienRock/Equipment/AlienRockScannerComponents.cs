using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Handheld tool displaying the nodes present on an artifact
/// </summary>
[RegisterComponent]
public sealed partial class AlienRockScannerComponent : Component
{
    /// <summary>
    /// Maximum range, in tiles, to display nodes on the artifact
    /// </summary>
    [DataField]
    public int Range = 5;

    /// <summary>
    /// update rate used by the UI controller
    /// </summary>
    [DataField]
    public TimeSpan UiUpdateRate = TimeSpan.FromSeconds(1);
}

/// <summary>
/// Marker component holding data for an active <see cref="AlienRockScannerComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(true), AutoGenerateComponentPause]
public sealed partial class AlienRockScannerConnectedComponent : Component
{
    /// <summary>
    /// Rock the scanner is currently scanning
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid AttachedTo;

    /// <summary>
    /// TBA
    /// </summary>
    [DataField]
    public TimeSpan UpdateInterval = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Next UI update time
    /// </summary>
    [DataField]
    public TimeSpan UpdateNext = TimeSpan.Zero;
}

[Serializable, NetSerializable]
public enum AlienRockScannerUiKey : byte
{
    Key
}
