using System;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:name")]
public class MoonNameCondition : MultipleCondition<SelectableLevel> {
	protected override string ValidateWarnMessage => $"Value field for a MoonNameCondition in SoundPack '{Pack.Name}' is empty or missing!";

	/// <inheritdoc/>
	public override void OnRegistered() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
		StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;

	/// <inheritdoc/>
	protected override bool TryCacheValue(out SelectableLevel value, string match) {
		value = null;

		if (!StartOfRound.Instance) return false;
		if (StartOfRound.Instance.levels == null) return false;
		if (string.IsNullOrEmpty(match)) return false;

		value = Array.Find(StartOfRound.Instance.levels, level => level != null &&
			string.Equals(level.name, match, StringComparison.InvariantCultureIgnoreCase));
		return value != null;
	}

	/// <inheritdoc/>
	protected override bool TryObtainValue(out SelectableLevel value, IContext context) {
		value = null;
		if (SceneManager.loadedSceneCount <= 0) return false;
		if (!StartOfRound.Instance) return false;
		value = StartOfRound.Instance.currentLevel;
		return value != null;
	}
}