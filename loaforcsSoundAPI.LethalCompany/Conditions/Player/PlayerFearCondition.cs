using JetBrains.Annotations;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:fear")]
public class PlayerFearCondition : Condition {
    public RangeOperator<float> Value { get; private set; } = new(0.0f, float.PositiveInfinity);

    public bool? IsIncreasing { get; private set; } = null;

    [CanBeNull]
    public RangeOperator<float> TimeSinceIncrease { get; private set; } = new(0.0f, float.PositiveInfinity);

    /// <inheritdoc/>
    public override bool Evaluate(IContext context) {
        if (!StartOfRound.Instance) return false;
        if (!GameNetworkManager.Instance) return false;
        if (!GameNetworkManager.Instance.localPlayerController) return false;
        if (GameNetworkManager.Instance.localPlayerController.isPlayerDead) return false;

        bool result = Value.EvaluateRange(StartOfRound.Instance.fearLevel);
        if (result && IsIncreasing.HasValue) {
            result = StartOfRound.Instance.fearLevelIncreasing == IsIncreasing.Value;
        }
        if (result && TimeSinceIncrease != null) {
            TimeSinceIncrease.EvaluateRange(GameNetworkManager.Instance.localPlayerController.timeSinceFearLevelUp);
        }

        return result;
    }
}