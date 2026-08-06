using Content.Shared.EntityTable;
using Robust.Shared.Containers;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Fills a container with node entities, and relays events to them.
/// Anchors & unanchors self based on presence of nodes.
/// </summary>
public sealed partial class AlienRockSystem : EntitySystem
{
    [Dependency] private SharedContainerSystem _container = default!;
    [Dependency] private EntityTableSystem _entityTable = default!;
    [Dependency] private SharedTransformSystem _xform = default!;

    public override void Initialize()
    {
        base.Initialize();
        InitializeRelay();
    }

    private void CheckAnchor(Entity<AlienRockComponent> ent)
    {
        var xform = Transform(ent);

        if (ent.Comp.Nodes!.Count == 0)
        {
            _xform.Unanchor(ent.Owner, xform);
            return;
        }

        _xform.AnchorEntity((ent.Owner, xform));
    }

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
            PredictedTrySpawnInContainer(
                protoName: node,
                containerUid: ent.Comp.Nodes.Owner,
                containerId: AlienRockComponent.ContainerId,
                uid: out _);
        }

        CheckAnchor(ent);
    }
}
