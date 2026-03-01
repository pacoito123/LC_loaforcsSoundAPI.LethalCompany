using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Moon;

[SoundAPICondition("LethalCompany:moon:current_time")]
public class CurrentTimeCondition : Condition {
    [CanBeNull]
    public string Value { get; internal set; } = null;

    public override bool Evaluate(IContext context) {
        if(TimeOfDay.Instance == null) return false;
        return false; // todo: do this lol
    }

    public override List<IValidatable.ValidationResult> Validate() {
        return !ValidateRangeOperator(Value, out IValidatable.ValidationResult result)
            ? [result] : [];
    }
}