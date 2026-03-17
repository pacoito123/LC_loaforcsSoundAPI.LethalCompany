using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Vehicle;

[SoundAPICondition("LethalCompany:vehicle:riding")]
public class VehicleRidingCondition : MultipleCondition<RiderType, VehicleContext> {
	/// <inheritdoc/>
    protected override bool TryGetValue(out RiderType riderType, string match) {
        return Enum.TryParse(match, ignoreCase: true, out riderType);
    }

	/// <inheritdoc/>
    protected override bool CheckValueWithContext(RiderType riderType, VehicleContext? context) {
        if(context == null || context.Vehicle == null || context.Vehicle.carDestroyed) return false;

        Vector3 playerPosition = Vector3.zero;
        if(riderType is RiderType.IN_BACK or RiderType.ON_TOP or RiderType.IN_FRONT or RiderType.NONE) {
            if(context.Vehicle.physicsRegion == null || context.Vehicle.boundsCollider == null || context.Vehicle.ontopOfTruckCollider == null) return false;
            if(GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null) return false;
            playerPosition = GameNetworkManager.Instance.localPlayerController.transform.position;
        }

        return riderType switch {
            RiderType.DRIVER => context.Vehicle.localPlayerInControl,
            RiderType.PASSENGER => context.Vehicle.localPlayerInPassengerSeat,
            RiderType.IN_BACK => context.Vehicle.physicsRegion.hasLocalPlayer
                && context.Vehicle.boundsCollider.bounds.Contains(playerPosition)
                && !context.Vehicle.ontopOfTruckCollider.bounds.Contains(playerPosition),
            RiderType.ON_TOP => context.Vehicle.physicsRegion.hasLocalPlayer
                && context.Vehicle.ontopOfTruckCollider.bounds.Contains(playerPosition),
            RiderType.IN_FRONT => context.Vehicle.physicsRegion.hasLocalPlayer
                && !context.Vehicle.boundsCollider.bounds.Contains(playerPosition)
                && !context.Vehicle.ontopOfTruckCollider.bounds.Contains(playerPosition),
            RiderType.NONE => !context.Vehicle.localPlayerInControl && !context.Vehicle.localPlayerInPassengerSeat
                && (!context.Vehicle.physicsRegion || !context.Vehicle.physicsRegion.hasLocalPlayer),
            _ => false,
        };
    }

	/// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        return _currentContext != null ? EvaluateWithContext(_currentContext)
            : VehicleContext.FallbackVehicle != null && EvaluateWithContext(new(VehicleContext.FallbackVehicle));
    }
}

public enum RiderType {
    NONE = -1,
    DRIVER,
    PASSENGER,
    IN_BACK,
    ON_TOP,
    IN_FRONT
}