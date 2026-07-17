using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class MoonsRegistry : Registry<SelectableLevel, HashSet<SelectableLevel>> {
    public MoonsRegistry() : base() => StartOfRoundPatch.StartOfRoundAwake += PopulateRegistry;

    /// <inheritdoc/>
    public override void OnRegistryPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateRegistry;

    /// <inheritdoc/>
    public override bool TryParse(out SelectableLevel value, JToken token) {
        value = null;

        if (!StartOfRound.Instance) return false;
        if (StartOfRound.Instance.levels == null) return false;

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        value = Array.Find(StartOfRound.Instance.levels, level => level != null &&
            string.Equals(level.name, match, StringComparison.InvariantCultureIgnoreCase));
        return value != null;
    }
}