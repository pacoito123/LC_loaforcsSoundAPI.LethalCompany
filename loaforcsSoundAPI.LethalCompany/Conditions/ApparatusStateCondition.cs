using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions;

[SoundAPICondition("LethalCompany:apparatus_state")]
public class ApparatusStateCondition : Condition {
	public enum StateType : byte {
		PULLED,
		PLUGGED_IN
	}
	internal static bool CurrentApparatusPulled = false;

	public StateType? Value { get; internal set; }

	public override bool Evaluate(IContext context) {
		return (CurrentApparatusPulled ? StateType.PULLED : StateType.PLUGGED_IN) == (Value ?? StateType.PULLED);
	}
}