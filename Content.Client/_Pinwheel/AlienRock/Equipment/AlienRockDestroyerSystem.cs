using Content.Client._Pinwheel.AlienRock.Ui;
using Content.Shared._Pinwheel.AlienRock.Equipment;
using Robust.Client.GameObjects;

namespace Content.Client._Pinwheel.AlienRock.Equipment;

/// <inheritdoc />
public sealed partial class AlienDestroyerSystem : SharedAlienDestroyerSystem
{
    [Dependency] private UserInterfaceSystem _ui = default!;

    [SubscribeLocalEvent]
    private void OnConsoleAfterAutoHandleState(Entity<AlienDestroyerConsoleComponent> ent,
        ref AfterAutoHandleStateEvent args)
    {
        UpdateBuiIfCanGetAnalysisConsoleUi(ent);
    }

    [SubscribeLocalEvent]
    private void OnDestroyerAfterAutoHandleState(Entity<AlienDestroyerComponent> ent,
        ref AfterAutoHandleStateEvent args)
    {
        if (!TryGetConsole(ent, out var console))
            return;

        UpdateBuiIfCanGetAnalysisConsoleUi(console.Value);
    }

    private void UpdateBuiIfCanGetAnalysisConsoleUi(Entity<AlienDestroyerConsoleComponent> ent)
    {
        if (_ui.TryGetOpenUi<AlienDestroyerConsoleBoundUserInterface>(
            ent.Owner,
            AlienDestroyerConsoleUiKey.Key, out var bui))
            bui.Update(ent);
    }
}
