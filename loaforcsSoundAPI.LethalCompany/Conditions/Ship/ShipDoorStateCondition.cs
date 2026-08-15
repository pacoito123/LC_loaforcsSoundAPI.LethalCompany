using JetBrains.Annotations;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Ship;

[SoundAPICondition("LethalCompany:ship:door_state")]
public class ShipDoorStateCondition : Condition<ShipDoorContext> {
    [CanBeNull]
    public bool? Closed { get; private set; }

    [CanBeNull]
    public RangeOperator<float> DoorPower { get; private set; } = new(0.0f, 100.0f);

    public bool? Overheated { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(ShipDoorContext context) {
        if (!context.ShipDoor) return false;

        bool result = !Closed.HasValue || StartOfRound.Instance.hangarDoorsClosed == Closed.Value;
        if (result && DoorPower != null) {
            result = DoorPower.EvaluateRange(context.ShipDoor.doorPower * 100.0f); // Evaluate value as a percentage.
        }
        if (result && Overheated.HasValue) {
            result = context.ShipDoor.overheated == Overheated.Value;
        }

        return result;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!ShipDoorContext.FallbackShipDoor) return false;

        return EvaluateWithContext(new ShipDoorContext(context?.Source, ShipDoorContext.FallbackShipDoor));
    }
}