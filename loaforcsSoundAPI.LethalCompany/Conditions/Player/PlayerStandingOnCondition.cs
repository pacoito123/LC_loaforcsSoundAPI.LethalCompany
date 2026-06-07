using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:standing_on")]
public class PlayerStandingOnCondition : MultipleCondition<FootstepSurface, PlayerContext> {
	protected override string ValidateWarnMessage => $"Value field for a PlayerStandingOnCondition in SoundPack '{Pack.Name}' is empty or missing!";

	/// <inheritdoc/>
	public override void OnRegistered() {
		StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
		StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
	}

	/// <inheritdoc/>
	protected override void OnValuesPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;

	/// <inheritdoc/>
	protected override bool TryCacheValue(out FootstepSurface value, string match) {
		value = null;

		if (!StartOfRound.Instance) return false;
		if (StartOfRound.Instance.footstepSurfaces == null) return false;
		if (string.IsNullOrEmpty(match)) return false;

		value = Array.Find(StartOfRound.Instance.footstepSurfaces, surface => surface != null
			&& string.Equals(surface.surfaceTag, match, StringComparison.InvariantCultureIgnoreCase));
		return value != null;
	}

	/// <inheritdoc/>
	protected override bool TryObtainValueWithContext(out FootstepSurface value, PlayerContext context) {
		value = null;
		if (!context.Player) return false;
		if (!StartOfRound.Instance) return false;

		FootstepSurface[] footstepSurfaces = StartOfRound.Instance.footstepSurfaces;
		if (footstepSurfaces == null) return false;

		int currentFootstepSurfaceIndex = context.Player.currentFootstepSurfaceIndex;
		if (currentFootstepSurfaceIndex < 0 || currentFootstepSurfaceIndex > footstepSurfaces.Length) return false;

		value = footstepSurfaces[currentFootstepSurfaceIndex];
		return value != null;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}
}