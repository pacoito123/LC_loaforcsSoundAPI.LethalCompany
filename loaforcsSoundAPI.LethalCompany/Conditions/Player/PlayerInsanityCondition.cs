using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:insanity")]
public class PlayerInsanityCondition : RangeCondition<float, PlayerContext> {
	/// <inheritdoc/>
	protected override RangeOperator<float> DefaultRange => new(float.NegativeInfinity, float.PositiveInfinity);

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		if (!context.Player) return false;
		if (context.Player.isPlayerDead) return false;

		return EvaluateRangeOperator(context.Player.insanityLevel);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		if (!GameNetworkManager.Instance.localPlayerController) return false;

		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}

	/// <inheritdoc/>
	protected override bool TryParseValue(string parameter, ref float value) {
		return string.IsNullOrEmpty(parameter) || float.TryParse(parameter, out value);
	}
}