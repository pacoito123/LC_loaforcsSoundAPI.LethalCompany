using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:delivery")]
public class ShipDeliveryCondition : Condition<DropshipContext> {
    public List<EnumReference<DropshipStateType>> Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(DropshipContext context) {
        if (!context.Dropship) return false;

        DropshipStateType currentDropshipState = (context.Dropship.deliveringOrder && !context.Dropship.deliveringVehicle) ? DropshipStateType.DELIVERING_ITEMS
            : context.Dropship.shipLanded ? DropshipStateType.DROPSHIP_LANDED
            : context.Dropship.shipDoorsOpened ? DropshipStateType.DROPSHIP_OPENED
            : context.Dropship.deliveringVehicle && !context.Dropship.untetheredVehicle ? DropshipStateType.DELIVERING_VEHICLE
            : context.Dropship.untetheredVehicle ? DropshipStateType.DROPPED_VEHICLE
            : DropshipStateType.LEAVING;

        return Value.FindIndex(reference => reference.Value == currentDropshipState) != -1;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!DropshipContext.FallbackDropship) return false;

        return EvaluateWithContext(new DropshipContext(context?.Source, DropshipContext.FallbackDropship));
    }

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:ship:delivery\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}

public enum DropshipStateType : byte {
    DELIVERING_ITEMS,
    DROPSHIP_LANDED,
    DROPSHIP_OPENED,
    DELIVERING_VEHICLE,
    DROPPED_VEHICLE,
    LEAVING
}