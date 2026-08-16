using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:weather:name")]
public class WeatherNameCondition : Condition {
    public List<WeatherContentReference> Value { get; private set; }

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        if (SceneManager.loadedSceneCount <= 1) return false;
        if (!StartOfRound.Instance) return false;
        if (!StartOfRound.Instance.currentLevel) return false;

        return Value.FindIndex(it => it.Value == StartOfRound.Instance.currentLevel.currentWeather) != -1;
    }

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:weather:name\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}