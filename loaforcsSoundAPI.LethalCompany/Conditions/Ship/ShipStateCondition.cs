using System;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:state")]
public class ShipStateCondition : MultipleCondition<ShipStateType> {
	protected override string ValidateWarnMessage => $"Value field for a ShipStateCondition in SoundPack '{Pack.Name}' is empty or missing!";

	/// <inheritdoc/>
	protected override bool TryCacheValue(out ShipStateType value, string match) {
		return Enum.TryParse(match, ignoreCase: true, out value);
	}

	/// <inheritdoc/>
	protected override bool TryObtainValue(out ShipStateType value, IContext context) {
		value = default;
		if (!StartOfRound.Instance) return false;

		value = StartOfRound.Instance.inShipPhase ? ShipStateType.IN_ORBIT
			: StartOfRound.Instance.shipIsLeaving ? ShipStateType.LEAVING
			: StartOfRound.Instance.shipHasLanded ? ShipStateType.LANDED
			: ShipStateType.LANDING;
		return true;
	}
}

public enum ShipStateType : byte {
	IN_ORBIT,
	LANDING,
	LANDED,
	LEAVING
}