using Content.Shared._Pinwheel.AlienRock;
using Robust.Shared.Audio.Systems;

namespace Content.Server._Pinwheel.AlienRock;

/// <inheritdoc />
public sealed partial class AlienRockDestroyerSystem : SharedAlienRockDestroyerSystem
{
    [Dependency] private SharedAudioSystem _audio = default!;

    [SubscribeLocalEvent]
    private void OnExtractButtonPressed(Entity<AlienRockConsoleComponent> ent,
        ref AlienRockConsoleButtonPressedMessage args)
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
