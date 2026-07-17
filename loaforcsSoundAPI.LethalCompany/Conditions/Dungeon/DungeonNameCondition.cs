using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:name")]
public class DungeonNameCondition : Condition {
	public DungeonsRegistry Value { get; private set; }

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		if (SceneManager.loadedSceneCount <= 1) return false;
		if (!RoundManager.Instance) return false;
		if (!RoundManager.Instance.dungeonGenerator) return false;

		return Value.ContainsValue(RoundManager.Instance.dungeonGenerator.Generator?.DungeonFlow);
	}
}