using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:state")]
public class ShipStateCondition : Condition {
	public List<EnumReference<ShipStateType>> Value { get; private set; }

	public override bool Evaluate(IContext context) {
		if (!StartOfRound.Instance) return false;

		ShipStateType currentShipState = StartOfRound.Instance.inShipPhase ? ShipStateType.IN_ORBIT
			: StartOfRound.Instance.shipIsLeaving ? ShipStateType.LEAVING
			: StartOfRound.Instance.shipHasLanded ? ShipStateType.LANDED
			: ShipStateType.LANDING;

		return Value.FindIndex(reference => reference.Value == currentShipState) != -1;
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:ship:state\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}

public enum ShipStateType : byte {
	IN_ORBIT,
	LANDING,
	LANDED,
	LEAVING
}