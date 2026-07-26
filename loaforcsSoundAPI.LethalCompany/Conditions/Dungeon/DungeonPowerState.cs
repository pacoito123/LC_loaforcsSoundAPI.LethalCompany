using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:power_state")]
public class DungeonPowerStateCondition : Condition {
	public static bool CurrentPowerState { get; internal set; }

	[CanBeNull]
	public bool? Value { get; private set; }

	public override bool Evaluate(IContext context) {
		return CurrentPowerState == (Value ?? true);
	}
}