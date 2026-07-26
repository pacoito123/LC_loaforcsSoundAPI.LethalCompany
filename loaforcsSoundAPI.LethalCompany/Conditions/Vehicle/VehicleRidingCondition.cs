using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Vehicle;

[SoundAPICondition("LethalCompany:vehicle:riding")]
public class VehicleRidingCondition : Condition<VehicleContext> {
    public List<EnumReference<RiderType>> Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(VehicleContext context) {
        if (!context.Vehicle) return false;
        if (context.Vehicle.carDestroyed) return false;
        if (context.Vehicle.vehicleID != 0) return false; // TODO: Vehicle name filtering, for modded vehicles. Condition supports only vanilla Cruiser for now.

        RiderType currentRiderType = context.Vehicle.localPlayerInControl ? RiderType.DRIVER
            : context.Vehicle.localPlayerInPassengerSeat ? RiderType.PASSENGER
            : context.Vehicle.physicsRegion.hasLocalPlayer ? RiderType.IN_BACK // TODO: Can match while on top or at the front.
            : RiderType.NONE;

        return Value.FindIndex(reference => reference.Value == currentRiderType) != -1;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!VehicleContext.FallbackVehicle) return false;

        return EvaluateWithContext(new VehicleContext(context?.Source, VehicleContext.FallbackVehicle));
    }

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:vehicle:riding\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
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