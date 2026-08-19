using Content.Client._Pinwheel.AlienRock.Ui;
using Content.Shared._Pinwheel.AlienRock;
using Robust.Client.GameObjects;

namespace Content.Client._Pinwheel.AlienRock;

/// <inheritdoc />
public sealed partial class AlienRockDestroyerSystem : SharedAlienRockDestroyerSystem
{
    [Dependency] private UserInterfaceSystem _ui = default!;

    [SubscribeLocalEvent]
    private void OnConsoleAfterAutoHandleState(Entity<AlienRockConsoleComponent> ent,
        ref AfterAutoHandleStateEvent args)
    {
        UpdateBuiIfCanGetAnalysisConsoleUi(ent);
    }

    [SubscribeLocalEvent]
    private void OnDestroyerAfterAutoHandleState(Entity<AlienRockDestroyerComponent> ent,
        ref AfterAutoHandleStateEvent args)
    {
        if (!TryGetConsole(ent, out var console))
            return;

        UpdateBuiIfCanGetAnalysisConsoleUi(console.Value);
    }

    private void UpdateBuiIfCanGetAnalysisConsoleUi(Entity<AlienRockConsoleComponent> ent)
    {
        if (_ui.TryGetOpenUi<AlienRockConsoleBoundUserInterface>(
            ent.Owner,
            AlienRockConsoleUiKey.Key, out var bui))
            bui.Update(ent);
    }
}
