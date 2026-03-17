using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:time_of_day")]
public class TimeOfDayCondition : MultipleCondition<DayMode> {
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
	protected override bool TryGetValue(out DayMode value, string match) {
		return Enum.TryParse(match, true, out value);
	}

	/// <inheritdoc/>
	protected override bool CheckValue(DayMode value) {
		return SceneManager.loadedSceneCount > 1 && TimeOfDay.Instance != null && TimeOfDay.Instance.dayMode == value;
	}
}