using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:delivery")]
public class ShipDeliveryCondition : Condition<DropshipContext> {
    public EnumsRegistry<DropshipStateType> Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(DropshipContext context) {
        if (!context.Dropship) return false;

        DropshipStateType currentDropshipState = (context.Dropship.deliveringOrder && !context.Dropship.deliveringVehicle) ? DropshipStateType.DELIVERING_ITEMS
            : context.Dropship.shipLanded ? DropshipStateType.DROPSHIP_LANDED
            : context.Dropship.shipDoorsOpened ? DropshipStateType.DROPSHIP_OPENED
            : context.Dropship.deliveringVehicle && !context.Dropship.untetheredVehicle ? DropshipStateType.DELIVERING_VEHICLE
            : context.Dropship.untetheredVehicle ? DropshipStateType.DROPPED_VEHICLE
            : DropshipStateType.LEAVING;

        return Value.ContainsValue(currentDropshipState);
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!DropshipContext.FallbackDropship) return false;

        return EvaluateWithContext(new DropshipContext(context?.Source, DropshipContext.FallbackDropship));
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