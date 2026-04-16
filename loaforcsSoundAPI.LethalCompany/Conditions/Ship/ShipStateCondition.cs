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
		return StartOfRound.Instance != null && value switch {
			ShipStateType.IN_ORBIT => StartOfRound.Instance.inShipPhase,
			ShipStateType.LANDING => !StartOfRound.Instance.inShipPhase && !StartOfRound.Instance.shipIsLeaving && !StartOfRound.Instance.shipHasLanded,
			ShipStateType.LANDED => !StartOfRound.Instance.inShipPhase && !StartOfRound.Instance.shipIsLeaving && StartOfRound.Instance.shipHasLanded,
			ShipStateType.LEAVING => !StartOfRound.Instance.inShipPhase && StartOfRound.Instance.shipIsLeaving,
			_ => false,
		};
	}
}

public enum ShipStateType : byte {
	IN_ORBIT,
	LANDING,
	LANDED,
	LEAVING
}