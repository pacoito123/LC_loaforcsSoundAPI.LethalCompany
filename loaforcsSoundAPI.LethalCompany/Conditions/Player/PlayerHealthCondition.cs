using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:health")]
public class PlayerHealthCondition : Condition {
	public RangeOperator<int> Value { get; private set; } = new(int.MinValue, int.MaxValue);

	/// <inheritdoc/>
	public override bool Evaluate(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		if (!GameNetworkManager.Instance.localPlayerController) return false;
		if (GameNetworkManager.Instance.localPlayerController.isPlayerDead) return false;

		return Value.EvaluateRange(GameNetworkManager.Instance.localPlayerController.health);
	}
}