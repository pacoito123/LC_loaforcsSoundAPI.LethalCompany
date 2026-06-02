using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:delivery")]
public class ShipDeliveryCondition : MultipleCondition<DropshipStateType, DropshipContext> {
	protected override bool TryGetValue(out DropshipStateType value, string match) {
		return Enum.TryParse(match, ignoreCase: true, out value);
	}

	protected override bool CheckValueWithContext(DropshipStateType value, DropshipContext context) {
		return context.Dropship != null && value switch {
			DropshipStateType.DELIVERING_ITEMS => context.Dropship.deliveringOrder && !context.Dropship.deliveringVehicle,
			DropshipStateType.DROPSHIP_LANDED => context.Dropship.shipLanded,
			DropshipStateType.DROPSHIP_OPENED => context.Dropship.shipDoorsOpened,
			DropshipStateType.DELIVERING_VEHICLE => context.Dropship.deliveringVehicle && !context.Dropship.untetheredVehicle,
			DropshipStateType.DROPPED_VEHICLE => context.Dropship.untetheredVehicle,
			DropshipStateType.LEAVING => !context.Dropship.deliveringOrder,
			_ => false,
		};
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return DropshipContext.FallbackDropship != null
			&& EvaluateWithContext(new(context?.Source, DropshipContext.FallbackDropship));
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