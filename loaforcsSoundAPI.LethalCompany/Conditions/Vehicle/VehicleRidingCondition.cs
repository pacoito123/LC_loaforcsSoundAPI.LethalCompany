using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Vehicle;

[SoundAPICondition("LethalCompany:vehicle:riding")]
public class VehicleRidingCondition : MultipleCondition<RiderType, VehicleContext> {
    protected override string ValidateWarnMessage => $"Value field for a VehicleRidingCondition in SoundPack '{Pack.Name}' is empty or missing!";

    /// <inheritdoc/>
    protected override bool TryCacheValue(out RiderType value, string match) {
        return Enum.TryParse(match, ignoreCase: true, out value);
    }

    /// <inheritdoc/>
    protected override bool TryObtainValueWithContext(out RiderType value, VehicleContext context) {
        value = default;
        if (!context.Vehicle) return false;
        if (context.Vehicle.carDestroyed) return false;
        if (context.Vehicle.vehicleID != 0) return false; // TODO: Vehicle name filtering, for modded vehicles. Only Cruiser for now.

        value = context.Vehicle.localPlayerInControl ? RiderType.DRIVER
            : context.Vehicle.localPlayerInPassengerSeat ? RiderType.PASSENGER
            : context.Vehicle.physicsRegion.hasLocalPlayer ? RiderType.IN_BACK // TODO: Can match while on top or at the front.
            : RiderType.NONE;
        return true;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!VehicleContext.FallbackVehicle) return false;
        return EvaluateWithContext(new VehicleContext(context?.Source, VehicleContext.FallbackVehicle));
    }
}

public enum RiderType : byte {
    NONE,
    DRIVER,
    PASSENGER,
    IN_BACK,
    // ON_TOP,
    // IN_FRONT
}