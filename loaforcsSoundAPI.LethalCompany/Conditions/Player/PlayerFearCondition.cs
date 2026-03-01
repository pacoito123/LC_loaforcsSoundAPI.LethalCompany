using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:fear")]
public class PlayerFearCondition : Condition {
    public bool? IsIncreasing { get; private set; } = null;

    [CanBeNull]
    public string TimeSinceIncrease { get; private set; } = null;

    [CanBeNull]
    public string Value { get; private set; } = null;

    public override bool Evaluate(IContext context) {
        if(StartOfRound.Instance == null || GameNetworkManager.Instance == null || GameNetworkManager.Instance.localPlayerController == null) return false;
        if(GameNetworkManager.Instance.localPlayerController.isPlayerDead) return false;

        bool? result = null;

        if(IsIncreasing != null && result != false) {
            result = IsIncreasing != StartOfRound.Instance.fearLevelIncreasing;
        }

        if(Value != null) {
            result = EvaluateRangeOperator(StartOfRound.Instance.fearLevel, Value);
        }

        if(TimeSinceIncrease != null) {
            result = EvaluateRangeOperator(GameNetworkManager.Instance.localPlayerController.timeSinceFearLevelUp, TimeSinceIncrease);
        }

        return result == true;
    }

    public override List<IValidatable.ValidationResult> Validate() {
        return (Value != null && !ValidateRangeOperator(Value, out IValidatable.ValidationResult result)) || (TimeSinceIncrease != null
            && !ValidateRangeOperator(TimeSinceIncrease, out result)) ? [result] : [];
    }
}