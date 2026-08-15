using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:insanity")]
public class PlayerInsanityCondition : Condition {
	public RangeOperator<float> Value { get; private set; } = new(float.NegativeInfinity, float.PositiveInfinity);

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		if (!GameNetworkManager.Instance.localPlayerController) return false;
		if (GameNetworkManager.Instance.localPlayerController.isPlayerDead) return false;

		return Value.EvaluateRange(GameNetworkManager.Instance.localPlayerController.insanityLevel);
	}
}