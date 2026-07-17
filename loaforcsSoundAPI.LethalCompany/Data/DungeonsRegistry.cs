using System;
using System.Collections.Generic;
using DunGen.Graph;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class DungeonsRegistry : Registry<DungeonFlow, HashSet<DungeonFlow>> {
    public DungeonsRegistry() : base() => RoundManagerPatch.OnRoundManagerAwake += PopulateRegistry;

	/// <inheritdoc/>
    public override void OnRegistryPopulated() => RoundManagerPatch.OnRoundManagerAwake -= PopulateRegistry;

	/// <inheritdoc/>
    public override bool TryParse(out DungeonFlow value, JToken token) {
        value = null;

        if (!RoundManager.Instance) return false;
        if (RoundManager.Instance.dungeonFlowTypes == null) return false;

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        value = Array.Find(RoundManager.Instance.dungeonFlowTypes, indoorMapType => indoorMapType?.dungeonFlow != null
            && string.Equals(indoorMapType.dungeonFlow.name, match, StringComparison.InvariantCultureIgnoreCase))?.dungeonFlow;

        return value != null;
    }
}