using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:state")]
public class ShipStateCondition : Condition {
	public enum ShipStateType {
		IN_ORBIT,
		LANDED
	}

	public ShipStateType Value { get; internal set; }

	public override bool Evaluate(IContext context) {
		return StartOfRound.Instance != null && (StartOfRound.Instance.inShipPhase
			? Value == ShipStateType.IN_ORBIT
			: Value == ShipStateType.LANDED);
	}
	// todo: validate
}