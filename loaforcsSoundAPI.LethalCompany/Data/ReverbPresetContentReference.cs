using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class ReverbPresetContentReference : ContentReference<ReverbPreset> {
    static ReverbPreset[] _allReverbPresets;
    static Dictionary<string, ReverbPreset> _cachedReverbPresets;

    public ReverbPresetContentReference(string input) : base(input) => StartOfRoundPatch.StartOfRoundAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved(bool success) => StartOfRoundPatch.StartOfRoundAwake -= Resolve;

    /// <inheritdoc/>
	protected override bool TryResolve(string input, out ReverbPreset value) {
        value = null;

        _allReverbPresets ??= Resources.FindObjectsOfTypeAll<ReverbPreset>();
        _cachedReverbPresets ??= new(_allReverbPresets.Length);

        input = input.ToLowerInvariant();
        if (!_cachedReverbPresets.TryGetValue(input, out value)) {
            for (int i = _allReverbPresets.Length - 1; i >= 0; i--) {
                value = _allReverbPresets[i];
                if (value != null && string.Equals(value.name, input, StringComparison.InvariantCultureIgnoreCase)
                    && _cachedReverbPresets.TryAdd(input, value)) break;
                value = null;
            }
        }

        return value != null;
    }
}