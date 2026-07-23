using System.Collections.Generic;
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

		return Value.Find(reference => reference.Value == StartOfRound.Instance.currentLevel) != null;
	}
}