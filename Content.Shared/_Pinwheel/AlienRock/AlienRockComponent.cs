using Content.Shared.EntityTable.EntitySelectors;
using Robust.Shared.Containers;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// TBA
/// </summary>
[RegisterComponent]
public sealed partial class AlienRockComponent : Component
{
    /// <summary>
    /// Table of nodes to spawn on mapinit
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityTableSelector NodeTable = new NoneSelector();

    public const string ContainerId = "nodes";

    /// <summary>
    /// Container holding nodes
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public Container? Nodes;
}
