using Content.Shared.Chemistry;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;

namespace Content.Shared._Pinwheel.AlienRock;

[RegisterComponent]
public sealed partial class AlienNodeReactiveComponent : Component
{
    [DataField]
    public List<ReactionMethod> ReactionMethods = new() { ReactionMethod.Touch };

    /// <summary>
    /// Reagents that are required in quantity <see cref="MinQuantity"/> to activate trigger.
    /// If any of them are present in required amount - activation will be triggered.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<ReagentPrototype>> Reagents = new();

    /// <summary>
    /// ReagentGroups that are required in quantity <see cref="MinQuantity"/> to activate trigger.
    /// If any of them are present in required amount - activation will be triggered.
    /// </summary>
    [DataField]
    public HashSet<ProtoId<ReactiveGroupPrototype>> ReactiveGroups = new();

    /// <summary>
    /// Min amount of reagent to trigger.
    /// </summary>
    [DataField]
    public FixedPoint2 MinQuantity = 5f;

}

public sealed partial class AlienNodeReactiveSystem : AlienNodeBaseSystem
{
    [SubscribeLocalEvent]
    private void OnReaction(Entity<AlienNodeReactiveComponent> node,
        ref AlienRockRelayedEvent<ReactionEntityEvent> rel)
    {
        if (!node.Comp.ReactionMethods.Contains(rel.Args.Method))
            return;

        if (rel.Args.ReagentQuantity.Quantity < node.Comp.MinQuantity)
            return;

        if (!node.Comp.Reagents.Contains(rel.Args.Reagent.ID))
            return;

        if (node.Comp.ReactiveGroups?.Count > 0 && !ReagentHaveReactiveGroup(rel.Args, node.Comp))
            return;

        NodeRemove(node.Owner);
    }

    private static bool ReagentHaveReactiveGroup(ReactionEntityEvent args, AlienNodeReactiveComponent comp)
    {
        var reactiveReagentEffectEntries = args.Reagent.ReactiveEffects;
        if (reactiveReagentEffectEntries == null)
        {
            return false;
        }

        var reactiveGroups = comp.ReactiveGroups;
        foreach(var reactiveGroup in reactiveGroups)
        {
            if (reactiveReagentEffectEntries.TryGetValue(reactiveGroup, out var effectEntry)
                && effectEntry.Methods?.Contains(args.Method) == true)
            {
                return true;
            }
        }

        return false;
    }

}
