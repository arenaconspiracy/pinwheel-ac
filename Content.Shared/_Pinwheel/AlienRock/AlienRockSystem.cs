using Content.Shared.EntityTable;
using Content.Shared.EntityTable.Conditions;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;
using System.Linq;

namespace Content.Shared._Pinwheel.AlienRock;

/// <summary>
/// Fills a container with node entities, and relays events to them.
/// Anchors & unanchors self based on presence of nodes.
/// </summary>
public sealed partial class AlienRockSystem : EntitySystem
{
    [Dependency] private SharedContainerSystem _container = default!;
    [Dependency] private EntityTableSystem _entityTable = default!;
    [Dependency] private SharedPointLightSystem _light = default!;
    [Dependency] private SharedTransformSystem _xform = default!;

    public override void Initialize()
    {
        base.Initialize();
        InitializeRelay(); // AlienRockSystem.Relay.cs
    }

    private void AdjustAnchor(Entity<AlienRockComponent> ent)
    {
        var xform = Transform(ent);

        if (!_container.TryGetContainer(ent.Owner, AlienRockComponent.ContainerId, out var nodes))
            return;

        if (nodes.Count == 0)
        {
            _xform.Unanchor(ent.Owner, xform);
            return;
        }

        _xform.AnchorEntity((ent.Owner, xform));
    }

    private void AdjustLight(Entity<AlienRockComponent> ent)
    {
        if (!_container.TryGetContainer(ent.Owner, AlienRockComponent.ContainerId, out var nodes))
            return;

        // BAD: magic numbers city
        var radius = ((Math.Pow(nodes.Count, 0.7)) + 1.5); // adding 1.5 because lights with a smaller radius become increasingly imperceptible

        _light.SetRadius(ent.Owner, (float)radius);
    }

    [SubscribeLocalEvent]
    private void OnMapInit(Entity<AlienRockComponent> ent, ref MapInitEvent args)
    {
        ent.Comp.Nodes = _container.EnsureContainer<Container>(ent, AlienRockComponent.ContainerId);

        var spawned = new HashSet<EntProtoId>();
        var ctx = new EntityTableContext(new Dictionary<string, object>
        {
            { UniqueCondition.UsedSpawnsKey, spawned },
        });


        for (int i = 0; i <= ent.Comp.NodeCount; i++)
        {
            var spawn = _entityTable.GetSpawns(ent.Comp.NodeTable, ctx: ctx).SingleOrDefault();
            spawned.Add(spawn);
            PredictedTrySpawnInContainer(
                protoName: spawn,
                containerUid: ent.Comp.Nodes.Owner,
                containerId: AlienRockComponent.ContainerId,
                uid: out _);
        }

        AdjustLight(ent);
        AdjustAnchor(ent);
    }

    [SubscribeLocalEvent]
    private void OnEntRemovedFromContainer(Entity<AlienRockComponent> ent,
        ref EntRemovedFromContainerMessage args)
    {
        AdjustLight(ent);
        AdjustAnchor(ent);
    }
}
