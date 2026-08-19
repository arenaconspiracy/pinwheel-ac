using Content.Shared._Pinwheel.AlienRock;
using Robust.Client.UserInterface;

namespace Content.Client._Pinwheel.AlienRock.Ui;

/// <summary>
/// BUI for the artifact destroyer console
/// </summary>
public sealed class AlienRockConsoleBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    [ViewVariables]
    private AlienRockConsoleMenu? _consoleMenu;

    /// <inheritdoc />
    protected override void Open()
    {
        base.Open();

        _consoleMenu = this.CreateWindow<AlienRockConsoleMenu>();
        _consoleMenu.SetOwner(Owner);

        _consoleMenu.OnClose += Close;
        _consoleMenu.OpenCentered();

        _consoleMenu.OnDestroyButtonPressed += () =>
        {
            SendMessage(new AlienRockConsoleButtonPressedMessage());
        };
    }

    /// <summary>
    /// Update UI state based on corresponding component.
    /// </summary>
    public void Update(Entity<AlienRockConsoleComponent> ent)
    {
        _consoleMenu?.Update(ent);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        _consoleMenu?.Dispose();
    }
}

