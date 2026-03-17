using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:fear")]
public class PlayerFearCondition : RangeCondition<float, PlayerContext> {
    /// <inheritdoc/>
    protected override RangeOperator<float> DefaultRange => new(float.NegativeInfinity, float.PositiveInfinity);

    public bool? IsIncreasing { get; private set; } = null;

    [CanBeNull]
    public string TimeSinceIncrease { get; private set; } = null!;

    public RangeOperator<float> TimeSinceIncreaseRange {
        get => _timeSinceIncreaseRange;
        private set => _timeSinceIncreaseRange = value;
    }
    private RangeOperator<float> _timeSinceIncreaseRange;

    /// <inheritdoc/>
    public override bool EvaluateWithContext(PlayerContext context) {
        if(context.Player == null || !context.Player.isPlayerDead || StartOfRound.Instance == null) return false; // TODO: Context maybe not needed?

        return EvaluateRangeOperator(StartOfRound.Instance.fearLevel) && IsIncreasing != !StartOfRound.Instance.fearLevelIncreasing
            && EvaluateRangeOperator(context.Player.timeSinceFearLevelUp, TimeSinceIncreaseRange);
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
            && EvaluateWithContext(new PlayerContext(GameNetworkManager.Instance.localPlayerController));
    }

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        return !string.IsNullOrEmpty(TimeSinceIncrease) && !ValidateRangeOperator(TimeSinceIncrease, out IValidatable.ValidationResult result)
            ? [result] : base.Validate();
    }

    /// <inheritdoc/>
    protected override bool TryParseValue(string parameter, ref float value) {
        return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
    }
}