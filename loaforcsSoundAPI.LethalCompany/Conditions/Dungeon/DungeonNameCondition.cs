using System;
using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:name")]
public class DungeonNameCondition : Condition {
	[CanBeNull]
	public string Value { get; internal set; } = null;

	public override bool Evaluate(IContext context) {
		return RoundManager.Instance != null && RoundManager.Instance.dungeonGenerator != null && RoundManager.Instance.dungeonGenerator.Generator.DungeonFlow != null
			&& string.Equals(Value, RoundManager.Instance.dungeonGenerator.Generator.DungeonFlow.name, StringComparison.InvariantCultureIgnoreCase);
	}

	// todo: validate
}