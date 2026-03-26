using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:insanity")]
public class PlayerInsanityCondition : RangeCondition<float, PlayerContext> {
	/// <inheritdoc/>
	protected override RangeOperator<float> DefaultRange => new(float.NegativeInfinity, float.PositiveInfinity);

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		return context.Player != null && !context.Player.isPlayerDead && EvaluateRangeOperator(context.Player.insanityLevel);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new PlayerContext(context.Source, GameNetworkManager.Instance.localPlayerController));
	}

	/// <inheritdoc/>
	protected override bool TryParseValue(string parameter, ref float value) {
		return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
	}
}