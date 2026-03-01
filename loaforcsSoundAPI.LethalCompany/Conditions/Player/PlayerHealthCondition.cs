using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:health")]
public class PlayerHealthCondition : Condition<PlayerContext> {
	[CanBeNull]
	public string Value { get; private set; } = null;

	protected override bool EvaluateWithContext(PlayerContext context) {
		return context.Player != null && !context.Player.isPlayerDead
			&& EvaluateRangeOperator(context.Player.health, Value);
	}

	protected override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new PlayerContext(GameNetworkManager.Instance.localPlayerController));
	}

	public override List<IValidatable.ValidationResult> Validate() {
		return !ValidateRangeOperator(Value, out IValidatable.ValidationResult result) ? [result] : [];
	}
}