using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Pinwheel.AlienRock.Equipment;

/// <summary>
/// Handheld tool displaying the nodes present on an artifact
/// </summary>
[RegisterComponent]
public sealed partial class AlienScannerComponent : Component
{
    /// <summary>
    /// Maximum range, in tiles, to display nodes on the artifact
    /// </summary>
    [DataField]
    public int Range = 5;

    /// <summary>
    /// Update rate of the UI controller
    /// </summary>
    [DataField]
    public TimeSpan UiUpdateRate = TimeSpan.FromSeconds(1);

    /// <summary>
    /// How long it takes to scan an artifact
    /// </summary>
    [DataField]
    public TimeSpan DoAfterLength = TimeSpan.FromSeconds(6);
}

/// <summary>
/// Marker component holding data for an active <see cref="AlienScannerComponent"/>
/// </summary>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(true), AutoGenerateComponentPause]
public sealed partial class AlienScannerConnectedComponent : Component
{
    /// <summary>
    /// Rock the scanner is currently scanning
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid Attached;

    /// <summary>
    /// Update rate of checking range from attached artifact
    /// </summary>
    [DataField]
    public TimeSpan UpdateRate = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Next UI update time
    /// </summary>
    [DataField]
    public TimeSpan UpdateNext = TimeSpan.Zero;
}

[Serializable, NetSerializable]
public enum AlienScannerUiKey : byte
{
    Key
}
