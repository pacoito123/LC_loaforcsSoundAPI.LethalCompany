using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:name")]
public class MoonNameCondition : MultipleCondition<SelectableLevel> {
	/// <inheritdoc/>
	protected override void OnRegistered() {
		if(string.IsNullOrEmpty(Value)) return;
		StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
	}

	/// <inheritdoc/>
	protected override bool TryGetValue(out SelectableLevel value, string match) {
		value = null!;

		if(StartOfRound.Instance == null || StartOfRound.Instance.levels == null) return false;
		value = Array.Find(StartOfRound.Instance.levels, level => level != null &&
			string.Equals(level.name, match, StringComparison.InvariantCultureIgnoreCase));

		return value != null;
	}

	/// <inheritdoc/>
	protected override bool CheckValue(SelectableLevel value) {
		return SceneManager.loadedSceneCount > 1 && StartOfRound.Instance != null && StartOfRound.Instance.currentLevel == value;
	}
}