using Robust.Client.UserInterface;

namespace Content.Client._Pinwheel.AlienRock.Ui;

/// <summary>
/// BUI for hand-held xeno artifact scanner,  server-provided UI updates.
/// </summary>
public sealed class AlienScannerBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private AlienScannerDisplay? _display;

    /// <inheritdoc />
    protected override void Open()
    {
        base.Open();

        _display = this.CreateWindow<AlienScannerDisplay>();
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
