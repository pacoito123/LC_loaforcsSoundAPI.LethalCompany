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
		if (!context.Player) return false;
		if (context.Player.isPlayerDead) return false;

		return EvaluateRangeOperator(context.Player.health);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		return EvaluateWithContext(new(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}

	/// <inheritdoc/>
	protected override bool TryParseValue(string parameter, ref int value) {
		return string.IsNullOrEmpty(parameter) || int.TryParse(parameter, out value);
	}
}