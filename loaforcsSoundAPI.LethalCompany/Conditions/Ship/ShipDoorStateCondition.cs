using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:door_state")]
public class ShipDoorStateCondition : Condition<ShipDoorContext> {
	[CanBeNull]
	public bool? Closed { get; internal set; }

	[CanBeNull]
	public string DoorPower { get; private set; } = null!;

	public RangeOperator<float> DoorPowerRange {
		get => _doorPowerRange;
		private set => _doorPowerRange = value;
	}
	private RangeOperator<float> _doorPowerRange;

	public bool? Overheated { get; internal set; }

	/// <inheritdoc/>
	public override bool EvaluateWithContext(ShipDoorContext context) {
		bool result = true;

		if(Closed.HasValue && StartOfRound.Instance != null)
			result = StartOfRound.Instance.hangarDoorsClosed == Closed.Value;

		if(context.ShipDoor != null) {
			if(result && !string.IsNullOrEmpty(DoorPower))
				result = DoorPowerRange.EvaluateRangeOperator(context.ShipDoor.doorPower * 100.0f); // Evaluate value as a percentage.
			if(result && Overheated.HasValue)
				result = context.ShipDoor.overheated == Overheated.Value;
		}

		return result;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return ShipDoorContext.FallbackShipDoor != null
			&& EvaluateWithContext(new(context.Source, ShipDoorContext.FallbackShipDoor));
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		return !string.IsNullOrEmpty(DoorPower) && !RangeOperator<float>.ValidateRangeOperator(DoorPower, out _doorPowerRange,
			out IValidatable.ValidationResult result, TryParseValue, new(0.0f, 100.0f)) ? [result] : [];
	}

	private bool TryParseValue(string parameter, ref float value) {
		return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
	}
}