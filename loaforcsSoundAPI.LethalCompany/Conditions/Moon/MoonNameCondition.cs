using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:name")]
public class MoonNameCondition : Condition {
	public List<MoonContentReference> Value { get; private set; }

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		if (SceneManager.loadedSceneCount <= 1) return false;
		if (!StartOfRound.Instance) return false;

		return Value.Find(it => it.Value == StartOfRound.Instance.currentLevel) != null;
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:moon:name\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}