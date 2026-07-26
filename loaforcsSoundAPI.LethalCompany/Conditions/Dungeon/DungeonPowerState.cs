using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Dungeon;

[SoundAPICondition("LethalCompany:dungeon:power_state")]
public class DungeonPowerStateCondition : Condition {
	internal static bool CurrentPowerState = false;

	public bool? Value { get; private set; }

	public override bool Evaluate(IContext context) {
		return CurrentPowerState == (Value ?? true);
	}
}