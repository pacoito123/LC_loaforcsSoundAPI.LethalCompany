using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:name")]
public class MoonNameCondition : MultipleCondition<SelectableLevel> {
	protected override void OnRegistered() {
		if(Value != null) StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	protected override void OnValuesPopulated() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
	}

	protected override bool TryGetValue(out SelectableLevel level, string match) {
		level = null!;

		if(StartOfRound.Instance == null) return false;
		level = Array.Find(StartOfRound.Instance.levels, level => level != null &&
			string.Equals(level.name, match, StringComparison.InvariantCultureIgnoreCase));

		return level != null;
	}

	protected override bool CheckValue(SelectableLevel level) {
		return SceneManager.loadedSceneCount > 1 && StartOfRound.Instance != null && StartOfRound.Instance.currentLevel == level;
	}
}