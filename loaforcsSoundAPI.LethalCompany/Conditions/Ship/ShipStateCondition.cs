using System;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:state")]
public class ShipStateCondition : MultipleCondition<ShipStateType> {
	/// <inheritdoc/>
	protected override bool TryGetValue(out ShipStateType value, string match) {
		return Enum.TryParse(match, ignoreCase: true, out value);
	}

	/// <inheritdoc/>
	protected override bool CheckValue(ShipStateType value, IContext context) {
		return StartOfRound.Instance != null
			&& (StartOfRound.Instance.inShipPhase ? value is ShipStateType.IN_ORBIT
			: StartOfRound.Instance.shipIsLeaving ? value is ShipStateType.LEAVING
			: StartOfRound.Instance.shipHasLanded ? value is ShipStateType.LANDED
			: value is ShipStateType.LANDING);
	}
}

public enum ShipStateType : byte {
	IN_ORBIT,
	LANDING,
	LANDED,
	LEAVING
}