using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class ReverbPresetsRegistry : Registry<ReverbPreset, HashSet<ReverbPreset>> {
    static ReverbPreset[] _allReverbPresets;
    static Dictionary<string, ReverbPreset> _cachedReverbPresets;

    public ReverbPresetsRegistry() : base() => StartOfRoundPatch.StartOfRoundAwake += PopulateRegistry;

    /// <inheritdoc/>
    public override void OnRegistryPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateRegistry;

    /// <inheritdoc/>
    public override void PopulateRegistry() {
        base.PopulateRegistry();
    }

    /// <inheritdoc/>
    public override bool TryParse(out ReverbPreset value, JToken token) {
        value = null;

        _allReverbPresets ??= Resources.FindObjectsOfTypeAll<ReverbPreset>();
        _cachedReverbPresets ??= new(_allReverbPresets.Length);

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        match = match.ToLowerInvariant();
        if (!_cachedReverbPresets.TryGetValue(match, out value)) {
            for (int i = _allReverbPresets.Length - 1; i >= 0; i--) {
                value = _allReverbPresets[i];
                if (value != null && string.Equals(value.name, match, StringComparison.InvariantCultureIgnoreCase)
                    && _cachedReverbPresets.TryAdd(match, value)) break;
                value = null;
            }
        }

        return value != null;
    }
}