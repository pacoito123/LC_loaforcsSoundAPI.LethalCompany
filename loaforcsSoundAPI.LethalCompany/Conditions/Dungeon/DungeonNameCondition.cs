using System;
using DunGen.Graph;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:name")]
public class DungeonNameCondition : MultipleCondition<DungeonFlow> {
	/// <inheritdoc/>
	protected override void OnRegistered() {
		if(string.IsNullOrEmpty(Value)) return;
		RoundManagerPatch.OnRoundManagerAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() {
		RoundManagerPatch.OnRoundManagerAwake -= PopulateValues;
	}

	/// <inheritdoc/>
	protected override bool TryGetValue(out DungeonFlow value, string match) {
		value = null!;

		if(RoundManager.Instance == null || RoundManager.Instance.dungeonFlowTypes == null) return false;
		value = Array.Find(RoundManager.Instance.dungeonFlowTypes, indoorMapType => indoorMapType?.dungeonFlow != null
			&& string.Equals(indoorMapType.dungeonFlow.name, match, StringComparison.InvariantCultureIgnoreCase))?.dungeonFlow!;

		return value != null;
	}

	/// <inheritdoc/>
	protected override bool CheckValue(DungeonFlow value) {
		return SceneManager.loadedSceneCount > 1 && RoundManager.Instance != null && RoundManager.Instance.dungeonGenerator != null
			&& RoundManager.Instance.dungeonGenerator.Generator != null && RoundManager.Instance.dungeonGenerator.Generator.DungeonFlow == value;
	}
}