using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class FootstepSurfaceContentReference : ContentReference<int> {
    public FootstepSurfaceContentReference(string input) : base(input) => StartOfRoundPatch.StartOfRoundAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved(bool success) => StartOfRoundPatch.StartOfRoundAwake -= Resolve;

    /// <inheritdoc/>
	protected override bool TryResolve(string input, out int value) {
        value = -1;

        if (!StartOfRound.Instance) return false;
        if (StartOfRound.Instance.footstepSurfaces == null) return false;
        if (string.IsNullOrEmpty(input)) return false;

        value = Array.FindIndex(StartOfRound.Instance.footstepSurfaces, it => string.Equals(it?.surfaceTag, input, StringComparison.InvariantCultureIgnoreCase));

        return value != -1;
    }
}