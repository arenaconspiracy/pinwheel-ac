using Content.Shared.EntityTable;
using Robust.Shared.Containers;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// TBA
/// </summary>
public sealed partial class AlienRockSystem : EntitySystem
{
    [Dependency] private SharedContainerSystem _container = default!;
    [Dependency] private EntityTableSystem _entityTable = default!;

    /// <summary>
    /// Ensures node container from ID,
    /// Fills container with nodes from table,
    /// Anchors itself to the floor
    /// </summary>
    [SubscribeLocalEvent]
    private void OnMapInit(Entity<AlienRockComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.Nodes =
            _container.EnsureContainer<Container>(ent, AlienRockComponent.ContainerId);

        var spawns = _entityTable.GetSpawns(ent.Comp.NodeTable);

        foreach (var node in spawns)
        {
            Log.Info($"{node} - {spawns}");
            PredictedTrySpawnInContainer(
                protoName: node,
                containerUid: ent.Comp.Nodes.Owner,
                containerId: AlienRockComponent.ContainerId,
                uid: out _);
        }
    }
}
