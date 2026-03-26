using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:health")]
public class PlayerHealthCondition : RangeCondition<int, PlayerContext> {
	/// <inheritdoc/>
	protected override RangeOperator<int> DefaultRange => new(int.MinValue, int.MaxValue);

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		return context.Player != null && !context.Player.isPlayerDead && EvaluateRangeOperator(context.Player.health);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new PlayerContext(context.Source, GameNetworkManager.Instance.localPlayerController));
	}

	/// <inheritdoc/>
	protected override bool TryParseValue(string parameter, ref int value) {
		return string.IsNullOrEmpty(parameter) || int.TryParse(parameter, out value);
	}
}