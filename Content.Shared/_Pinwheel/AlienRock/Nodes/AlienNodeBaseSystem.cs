using Robust.Shared.Containers;

namespace Content.Shared._Pinwheel.AlienRock;

public abstract partial class AlienNodeBaseSystem : EntitySystem
{
    [Dependency] private SharedContainerSystem _container = default!;

    /// <summary>
    /// Removes itself from the container to raise an event on the artifact
    /// and deletes itself
    /// </summary>
    protected void NodeRemove(EntityUid uid)
    {
        var xform = Transform(uid);

        _container.TryGetOuterContainer(uid, xform, out var container);
        _container.Remove(uid, container!);
        PredictedQueueDel(uid);
    }
}
