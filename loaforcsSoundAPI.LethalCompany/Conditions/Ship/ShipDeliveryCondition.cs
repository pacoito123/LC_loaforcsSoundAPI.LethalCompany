using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:delivery")]
public class ShipDeliveryCondition : MultipleCondition<DropshipStateType, DropshipContext> {
    protected override string ValidateWarnMessage => $"Value field for a ShipDeliveryCondition in SoundPack '{Pack.Name}' is empty or missing!";

    protected override bool TryCacheValue(out DropshipStateType value, string match) {
        return Enum.TryParse(match, ignoreCase: true, out value);
    }

    protected override bool TryObtainValueWithContext(out DropshipStateType value, DropshipContext context) {
        value = default;
        if (!context.Dropship) return false;

        value = (context.Dropship.deliveringOrder && !context.Dropship.deliveringVehicle) ? DropshipStateType.DELIVERING_ITEMS
            : context.Dropship.shipLanded ? DropshipStateType.DROPSHIP_LANDED
            : context.Dropship.shipDoorsOpened ? DropshipStateType.DROPSHIP_OPENED
            : context.Dropship.deliveringVehicle && !context.Dropship.untetheredVehicle ? DropshipStateType.DELIVERING_VEHICLE
            : context.Dropship.untetheredVehicle ? DropshipStateType.DROPPED_VEHICLE
            : DropshipStateType.LEAVING;
        return true;
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