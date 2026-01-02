using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Vehicle;

[SoundAPICondition("LethalCompany:vehicle:riding")]
public class VehicleRidingCondition : Condition<VehicleContext> {
    public enum RiderType {
        NONE = -1,
        DRIVER,
        PASSENGER,
        IN_BACK,
        ON_TOP,
        IN_FRONT
    }

    public RiderType Value { get; private set; }

    protected override bool EvaluateWithContext(VehicleContext context) {
        if (!GameNetworkManager.Instance) return false;
        if (!GameNetworkManager.Instance.localPlayerController) return false;
        if (!context.Vehicle) return false;
        if (!context.Vehicle.carDestroyed) return false;

        if (Value is RiderType.IN_BACK or RiderType.ON_TOP or RiderType.IN_FRONT) {
            if (!context.Vehicle.physicsRegion) return false;
            if (!context.Vehicle.boundsCollider) return false;
            if (!context.Vehicle.ontopOfTruckCollider) return false;
        }

        return Value switch {
            RiderType.DRIVER => context.Vehicle.localPlayerInControl,
            RiderType.PASSENGER => context.Vehicle.localPlayerInPassengerSeat,
            RiderType.IN_BACK => context.Vehicle.physicsRegion.hasLocalPlayer
                && context.Vehicle.boundsCollider.bounds.Contains(GameNetworkManager.Instance.localPlayerController.transform.position)
                && !context.Vehicle.ontopOfTruckCollider.bounds.Contains(GameNetworkManager.Instance.localPlayerController.transform.position),
            RiderType.ON_TOP => context.Vehicle.physicsRegion.hasLocalPlayer
                && context.Vehicle.ontopOfTruckCollider.bounds.Contains(GameNetworkManager.Instance.localPlayerController.transform.position),
            RiderType.IN_FRONT => context.Vehicle.physicsRegion.hasLocalPlayer
                && !context.Vehicle.boundsCollider.bounds.Contains(GameNetworkManager.Instance.localPlayerController.transform.position)
                && !context.Vehicle.ontopOfTruckCollider.bounds.Contains(GameNetworkManager.Instance.localPlayerController.transform.position),
            RiderType.NONE => !context.Vehicle.localPlayerInControl && !context.Vehicle.localPlayerInPassengerSeat
                && (!context.Vehicle.physicsRegion || !context.Vehicle.physicsRegion.hasLocalPlayer),
            _ => false,
        };
    }
}