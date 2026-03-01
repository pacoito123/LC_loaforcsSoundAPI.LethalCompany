using System;
using JetBrains.Annotations;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:standing_on")]
public class PlayerStandingOnCondition : Condition<PlayerContext> {
	[CanBeNull]
	public string Value { get; internal set; } = null;

	protected override bool EvaluateWithContext(PlayerContext context) {
		return context.Player != null && StartOfRound.Instance != null && context.Player.currentFootstepSurfaceIndex < StartOfRound.Instance.footstepSurfaces?.Length
			&& string.Equals(Value, StartOfRound.Instance.footstepSurfaces[context.Player.currentFootstepSurfaceIndex].surfaceTag, StringComparison.InvariantCultureIgnoreCase);
	}

	protected override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance && EvaluateWithContext(new PlayerContext(GameNetworkManager.Instance.localPlayerController));
	}
	// todo: validate
}