using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:standing_on")]
public class PlayerStandingOnCondition : Condition<PlayerContext> {
	public List<FootstepSurfaceContentReference> Value { get; private set; }

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		if (!context.Player) return false;

		return Value.Find(it => it.Value == context.Player.currentFootstepSurfaceIndex) != null;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		if (!GameNetworkManager.Instance.localPlayerController) return false;

		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}

	/// <inheritdoc/>
	public override List<IValidatable.ValidationResult> Validate() {
		if (Value == null || Value.Count == 0) {
			return [
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:player:standing_on\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}