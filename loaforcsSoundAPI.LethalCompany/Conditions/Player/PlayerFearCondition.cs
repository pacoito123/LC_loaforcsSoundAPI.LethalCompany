using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:fear")]
public class PlayerFearCondition : RangeCondition<float> {
    /// <inheritdoc/>
    protected override RangeOperator<float> DefaultRange => new(0.0f, float.PositiveInfinity);

    public bool? IsIncreasing { get; private set; } = null;

    [CanBeNull]
    public string TimeSinceIncrease { get; private set; } = null;

    public RangeOperator<float> TimeSinceIncreaseRange {
        get => _timeSinceIncreaseRange;
        private set => _timeSinceIncreaseRange = value;
    }
    RangeOperator<float> _timeSinceIncreaseRange;

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        bool result = true;

        if (StartOfRound.Instance != null) {
            if (IsIncreasing.HasValue) {
                result = StartOfRound.Instance.fearLevelIncreasing == IsIncreasing.Value;
            }
            if (result) {
                result = EvaluateRangeOperator(StartOfRound.Instance.fearLevel);
            }
        }

        if (GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null) {
            if (result && !GameNetworkManager.Instance.localPlayerController.isPlayerDead) {
                EvaluateRangeOperator(GameNetworkManager.Instance.localPlayerController.timeSinceFearLevelUp, TimeSinceIncreaseRange);
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        return !string.IsNullOrEmpty(TimeSinceIncrease) && !ValidateRangeOperator(TimeSinceIncrease, out _timeSinceIncreaseRange,
            out IValidatable.ValidationResult result) ? [result] : base.Validate();
    }

    /// <inheritdoc/>
    protected override bool TryParseValue(string parameter, ref float value) {
        return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
    }
}