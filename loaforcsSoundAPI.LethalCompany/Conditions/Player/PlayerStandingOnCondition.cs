using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:standing_on")]
public class PlayerStandingOnCondition : MultipleCondition<int, PlayerContext> {
	/// <inheritdoc/>
	protected override void OnRegistered() {
		if(string.IsNullOrEmpty(Value)) return;
		StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
	}

	/// <inheritdoc/>
	protected override bool TryGetValue(out int value, string match) {
		value = -1;

		if(StartOfRound.Instance == null || StartOfRound.Instance.footstepSurfaces == null) return false;
		value = Array.FindIndex(StartOfRound.Instance.footstepSurfaces, surface => surface != null
			&& string.Equals(surface.surfaceTag, match, StringComparison.InvariantCultureIgnoreCase));

		return value != -1;
	}

	/// <inheritdoc/>
	protected override bool CheckValueWithContext(int value, PlayerContext context) {
		return context.Player != null && context.Player.currentFootstepSurfaceIndex == value;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}
}