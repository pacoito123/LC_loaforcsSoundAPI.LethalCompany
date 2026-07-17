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
    public string DoorPower { get; private set; }

    public RangeOperator<float> DoorPowerRange {
        get => _doorPowerRange;
        private set => _doorPowerRange = value;
    }
    RangeOperator<float> _doorPowerRange;

    public bool? Overheated { get; internal set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(ShipDoorContext context) {
        if (!context.ShipDoor) return false;

        bool result = !Closed.HasValue || StartOfRound.Instance.hangarDoorsClosed == Closed.Value;
        if (result && !string.IsNullOrEmpty(DoorPower)) {
            result = DoorPowerRange.EvaluateRangeOperator(context.ShipDoor.doorPower * 100.0f); // Evaluate value as a percentage.
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

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        if (!string.IsNullOrEmpty(DoorPower)) {
            if (!RangeOperator<float>.ValidateRangeOperator(DoorPower, out _doorPowerRange, out IValidatable.ValidationResult result,
                TryParseValue, new RangeOperator<float>(0.0f, 100.0f))) return [result];
        }
        return base.Validate();
    }

    static bool TryParseValue(string parameter, ref float value) {
        return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
    }
}