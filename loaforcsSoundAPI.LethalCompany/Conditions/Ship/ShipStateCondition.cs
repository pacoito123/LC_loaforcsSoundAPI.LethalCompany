using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:state")]
public class ShipStateCondition : Condition {
	public EnumsRegistry<ShipStateType> Value { get; private set; }

	public override bool Evaluate(IContext context) {
		if (!StartOfRound.Instance) return false;
		ShipStateType currentShipState = StartOfRound.Instance.inShipPhase ? ShipStateType.IN_ORBIT
			: StartOfRound.Instance.shipIsLeaving ? ShipStateType.LEAVING
			: StartOfRound.Instance.shipHasLanded ? ShipStateType.LANDED
			: ShipStateType.LANDING;
		return Value.ContainsValue(currentShipState);
	}
}

public enum ShipStateType : byte {
	IN_ORBIT,
	LANDING,
	LANDED,
	LEAVING
}