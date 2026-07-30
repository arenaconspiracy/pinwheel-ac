using Content.Shared.StatusIcon;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.Traitor;

/// <summary>
/// Supplies a status icon between fellow traitors
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class TraitorComponent : Component
{
    /// <summary>
    /// The status icon prototype displayed for revolutionaries
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<FactionIconPrototype> StatusIcon { get; set; } = "SyndicateFaction";

    public override bool SessionSpecific => true;
}
