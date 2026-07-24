using System;
using DunGen.Graph;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class DungeonContentReference : ContentReference<DungeonFlow> {
    public DungeonContentReference(string input) : base(input) => RoundManagerPatch.OnRoundManagerAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved(bool success) => RoundManagerPatch.OnRoundManagerAwake -= Resolve;

    /// <inheritdoc/>
	protected override bool TryResolve(string input, out DungeonFlow value) {
        value = null;

        if (!RoundManager.Instance) return false;
        if (RoundManager.Instance.dungeonFlowTypes == null) return false;
        if (string.IsNullOrEmpty(input)) return false;

        value = Array.Find(RoundManager.Instance.dungeonFlowTypes, indoorMapType => indoorMapType?.dungeonFlow != null
            && string.Equals(indoorMapType.dungeonFlow.name, input, StringComparison.InvariantCultureIgnoreCase))?.dungeonFlow;

        return value != null;
    }
}