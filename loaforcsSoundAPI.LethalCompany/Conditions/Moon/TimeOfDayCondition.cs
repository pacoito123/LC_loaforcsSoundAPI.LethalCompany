using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:time_of_day")]
public class TimeOfDayCondition : MultipleCondition<DayMode> {
	protected override string ValidateWarnMessage => $"Value field for a TimeOfDayCondition in SoundPack '{Pack.Name}' is empty or missing!";

	/// <inheritdoc/>
	public override void OnRegistered() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
		StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
	}

	/// <inheritdoc/>
	protected override bool TryCacheValue(out DayMode value, string match) {
		return Enum.TryParse(match, true, out value);
	}

	/// <inheritdoc/>
	protected override bool TryObtainValue(out DayMode value, IContext context) {
		value = default;
		if (SceneManager.loadedSceneCount <= 1) return false;
		if (!TimeOfDay.Instance) return false;
		value = TimeOfDay.Instance.dayMode;
		return true;
	}
}