using Robust.Client.UserInterface;

namespace Content.Client._Pinwheel.AlienRock.Ui;

/// <summary>
/// BUI for hand-held xeno artifact scanner,  server-provided UI updates.
/// </summary>
public sealed class AlienRockScannerBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private AlienRockScanner? _display;

    /// <inheritdoc />
    protected override void Open()
    {
        base.Open();

        _display = this.CreateWindow<AlienRockScanner>();
        _display.SetOwner(Owner);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        _display?.Dispose();
    }
}
