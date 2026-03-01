using System;
using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:time_of_day")]
public class TimeOfDayCondition : Condition {
	[CanBeNull]
	public string Value { get; internal set; } = null;

	public override bool Evaluate(IContext context) {
		return TimeOfDay.Instance != null
			&& string.Equals(Value, $"{TimeOfDay.Instance.dayMode}", StringComparison.InvariantCultureIgnoreCase);
	}

	// todo: validate
}