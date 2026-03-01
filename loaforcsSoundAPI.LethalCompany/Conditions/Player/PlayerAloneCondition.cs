using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:is_alone")]
public class PlayerAloneCondition : Condition<PlayerContext> {
	public bool? Value { get; internal set; }

	protected override bool EvaluateWithContext(PlayerContext context) {
		return context.Player != null && context.Player.isPlayerAlone == (Value ?? true);
	}

	protected override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new PlayerContext(GameNetworkManager.Instance.localPlayerController));
	}
}