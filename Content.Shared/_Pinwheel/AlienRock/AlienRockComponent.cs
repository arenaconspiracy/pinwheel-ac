using Content.Shared.EntityTable.EntitySelectors;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Fills a container with node entities, and relays events to them.
/// Anchors & unanchors self based on presence of nodes.
/// </summary>
[RegisterComponent]
public sealed partial class AlienRockComponent : Component
{
    /// <summary>
    /// Table of nodes to spawn on mapinit
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public EntityTableSelector NodeTable = new NoneSelector();

    /// <summary>
    /// Amount of nodes to spawn
    /// </summary>
    /// <remarks>
    /// IN A PERFECT WORLD THIS WOULD BE DONE VIA EntityTableSelector.Rolls
    /// but the way UniqueCondition & EntityTableContext work we need to track individual results
    /// </remarks>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public int NodeCount = 6;

    /// <summary>
    /// Container holding nodes
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public Container? Nodes;
}
