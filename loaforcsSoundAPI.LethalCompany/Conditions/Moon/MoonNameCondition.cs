using System;
using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:name")]
public class MoonNameCondition : Condition {
	[CanBeNull]
	public string Value { get; internal set; } = null;

	public override bool Evaluate(IContext context) {
		return StartOfRound.Instance != null && StartOfRound.Instance.currentLevel != null
			&& string.Equals(Value, StartOfRound.Instance.currentLevel.name, StringComparison.InvariantCultureIgnoreCase);
	}

	// todo: validate
}