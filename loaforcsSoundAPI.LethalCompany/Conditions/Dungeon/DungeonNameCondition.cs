using System;
using DunGen.Graph;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:name")]
public class DungeonNameCondition : MultipleCondition<DungeonFlow> {
	protected override void OnRegistered() {
		if(Value != null) RoundManagerPatch.OnRoundManagerAwake += PopulateValues;
	}

	protected override void OnValuesPopulated() {
		RoundManagerPatch.OnRoundManagerAwake -= PopulateValues;
	}

	protected override bool TryGetValue(out DungeonFlow dungeon, string match) {
		dungeon = null!;

		if(RoundManager.Instance == null) return false;
		IndoorMapType? indoorMapType = Array.Find(RoundManager.Instance.dungeonFlowTypes, indoorMapType => indoorMapType.dungeonFlow != null
			&& string.Equals(indoorMapType.dungeonFlow.name, match, StringComparison.InvariantCultureIgnoreCase));
		if(indoorMapType == null) return false;

		dungeon = indoorMapType.dungeonFlow;
		return true;
	}

	protected override bool CheckValue(DungeonFlow dungeon) {
		return SceneManager.loadedSceneCount > 1 && RoundManager.Instance != null && RoundManager.Instance.dungeonGenerator != null
			&& RoundManager.Instance.dungeonGenerator.Generator != null && RoundManager.Instance.dungeonGenerator.Generator.DungeonFlow == dungeon;
	}
}