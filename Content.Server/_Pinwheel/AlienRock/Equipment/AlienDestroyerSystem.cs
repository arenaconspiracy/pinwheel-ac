using Content.Shared._Pinwheel.AlienRock;
using Content.Shared._Pinwheel.AlienRock.Equipment;
using Robust.Shared.Audio.Systems;

namespace Content.Server._Pinwheel.AlienRock.Equipment;

/// <inheritdoc />
public sealed partial class AlienDestroyerSystem : SharedAlienDestroyerSystem
{
    [Dependency] private SharedAudioSystem _audio = default!;

    [SubscribeLocalEvent]
    private void OnExtractButtonPressed(Entity<AlienDestroyerConsoleComponent> ent,
        ref AlienDestroyerConsoleButtonPressedMessage args)
    {
        if (!TryGetDestroyer(ent, out var destroyer))
            return;

        if (!TryGetArtifactFromConsole(ent, out var artifact))
            return;

        _audio.PlayPvs(
            destroyer.Value.Comp.DestroySound,
            destroyer.Value.Owner);
        TrySpawnNextTo(
            destroyer.Value.Comp.DestroyEffect,
            destroyer.Value.Owner,
            out EntityUid? _);
        PredictedQueueDel(artifact.Value.Owner);
    }
}
