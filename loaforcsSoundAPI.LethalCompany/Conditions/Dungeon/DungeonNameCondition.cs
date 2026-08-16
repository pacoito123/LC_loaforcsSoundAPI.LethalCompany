using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:name")]
public class DungeonNameCondition : Condition {
	public List<DungeonContentReference> Value { get; private set; }

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		if (SceneManager.loadedSceneCount <= 1) return false;
		if (!RoundManager.Instance) return false;
		if (!RoundManager.Instance.dungeonGenerator) return false;

		return Value.Find(static it => it.Value == RoundManager.Instance.dungeonGenerator.Generator?.DungeonFlow) != null;
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:dungeon:name\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}