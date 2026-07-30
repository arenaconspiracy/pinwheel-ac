namespace Content.Shared._Pinwheel.Traitor;

/// <summary>
/// Dummy system to let the client counterpart access <see cref="TraitorComponent"/>
/// </summary>
public abstract partial class SharedTraitorSystem : EntitySystem
{
    public override void Initialize()
    {
    }
}
