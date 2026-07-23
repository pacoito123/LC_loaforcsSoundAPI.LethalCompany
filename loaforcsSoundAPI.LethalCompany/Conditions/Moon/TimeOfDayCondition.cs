using System.Collections.Generic;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine.SceneManagement;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:time_of_day")]
public class TimeOfDayCondition : Condition {
	public List<EnumReference<DayMode>> Value { get; private set; }

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		if (SceneManager.loadedSceneCount <= 1) return false;
		if (!TimeOfDay.Instance) return false;

		return Value.FindIndex(static reference => reference.Value == TimeOfDay.Instance.dayMode) != -1;
	}
}