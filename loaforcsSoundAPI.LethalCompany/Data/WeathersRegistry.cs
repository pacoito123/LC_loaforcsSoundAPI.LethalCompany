using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class WeathersRegistry : Registry<LevelWeatherType, HashSet<LevelWeatherType>> {
    public WeathersRegistry() : base() => StartOfRoundPatch.StartOfRoundAwake += PopulateRegistry;

    /// <inheritdoc/>
    public override void OnRegistryPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateRegistry;

    /// <inheritdoc/>
    public override bool TryParse(out LevelWeatherType value, JToken token) {
        value = default;

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        return Enum.TryParse(match, ignoreCase: true, out value);
    }
}