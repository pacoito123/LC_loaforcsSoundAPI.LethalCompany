using System;
using DunGen.Graph;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:name")]
public class DungeonNameCondition : MultipleCondition<DungeonFlow> {
	protected override string ValidateWarnMessage => $"Value field for a DungeonNameCondition in SoundPack '{Pack.Name}' is empty or missing!";

	/// <inheritdoc/>
	public override void OnRegistered() {
		RoundManagerPatch.OnRoundManagerAwake -= PopulateValues;
		RoundManagerPatch.OnRoundManagerAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() => RoundManagerPatch.OnRoundManagerAwake -= PopulateValues;

	/// <inheritdoc/>
	protected override bool TryCacheValue(out DungeonFlow value, string match) {
		value = null;
		if (!RoundManager.Instance) return false;

		IndoorMapType[] dungeonFlowTypes = RoundManager.Instance.dungeonFlowTypes;
		if (dungeonFlowTypes == null) return false;

		value = Array.Find(RoundManager.Instance.dungeonFlowTypes, indoorMapType => indoorMapType?.dungeonFlow != null
			&& string.Equals(indoorMapType.dungeonFlow.name, match, StringComparison.InvariantCultureIgnoreCase))?.dungeonFlow;
		return value != null;
	}

	/// <inheritdoc/>
	protected override bool TryObtainValue(out DungeonFlow value, IContext context) {
		value = null;
		if (SceneManager.loadedSceneCount <= 1) return false;

		if (!RoundManager.Instance) return false;
		if (!RoundManager.Instance.dungeonGenerator) return false;

		value = RoundManager.Instance.dungeonGenerator.Generator?.DungeonFlow;
		return value != null;
	}
}