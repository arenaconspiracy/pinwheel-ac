using Content.Shared.DoAfter;
using Content.Shared.Tools;
using Content.Shared.Tools.Components;
using Content.Shared.Tools.Systems;
using Content.Shared.Interaction;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.AlienRock;

[RegisterComponent]
public sealed partial class AlienNodeToolUseComponent : Component
{
    [DataField]
    public ProtoId<ToolQualityPrototype> Quality = default!;

    [DataField]
    public float Delay = 6.0f; // NOT A TIMESPAN BECAUSE SharedToolSystem DOESN'T ACCEPT TIMESPANS GRRRRRRRRR

    [DataField]
    public int Fuel = 0;
}

public sealed partial class AlienNodeToolUseSystem : AlienNodeBaseSystem
{
    [Dependency] private SharedToolSystem _tool = default!;

    [SubscribeLocalEvent]
    private void OnToolUseComplete(Entity<AlienNodeToolUseComponent> node,
        ref AlienRockRelayedEvent<AlienNodeToolUseDoAfterEvent> rel)
    {
        if (rel.Args.Cancelled)
            return;

        if (rel.Args.Node == GetNetEntity(node))
            NodeRemove(node.Owner);
    }

    [SubscribeLocalEvent]
    private void OnInteractUsing(Entity<AlienNodeToolUseComponent> node,
        ref AlienRockRelayedEvent<InteractUsingEvent> rel)
    {
        if (!TryComp<ToolComponent>(rel.Args.Used, out var tool))
            return;

        rel.Args.Handled = _tool.UseTool(rel.Args.Used,
            rel.Args.User,
            rel.Artifact,
            node.Comp.Delay,
            [node.Comp.Quality],
            new AlienNodeToolUseDoAfterEvent(GetNetEntity(node)),
            fuel: node.Comp.Fuel,
            tool);
    }
}

[Serializable, NetSerializable]
public sealed partial class AlienNodeToolUseDoAfterEvent : DoAfterEvent
{
    public NetEntity Node;

    public AlienNodeToolUseDoAfterEvent(NetEntity node)
    {
        Node = node;
    }

    public override DoAfterEvent Clone()
    {
        return this;
    }
}
