using Content.Client.Overlays;
using Content.Shared.StatusIcon.Components;
using Content.Shared._Pinwheel.Traitor;

namespace Content.Client._Pinwheel.Traitor;

/// <summary>
/// Used for the client to get status icons from fellow traitors
/// </summary>
public sealed partial class TraitorSystem : EquipmentHudSystem<TraitorComponent>
{
    [SubscribeLocalEvent]
    private void GetStatusIcon(Entity<TraitorComponent> ent, ref GetStatusIconsEvent args)
    {
        if (ProtoMan.Resolve(ent.Comp.StatusIcon, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
    }
}
