using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class MoonContentReference : ContentReference<SelectableLevel> {
    public MoonContentReference(string input) : base(input) => StartOfRoundPatch.StartOfRoundAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved() => StartOfRoundPatch.StartOfRoundAwake -= Resolve;

    /// <inheritdoc/>
	protected override bool TryResolve(string input, out SelectableLevel value) {
        value = null;

        if (!StartOfRound.Instance) return false;
        if (StartOfRound.Instance.levels == null) return false;
        if (string.IsNullOrEmpty(input)) return false;

        value = Array.Find(StartOfRound.Instance.levels, level => level != null &&
            string.Equals(level.name, input, StringComparison.InvariantCultureIgnoreCase));

        return value != null;
    }
}