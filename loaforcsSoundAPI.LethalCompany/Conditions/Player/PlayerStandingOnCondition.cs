using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:standing_on")]
public class PlayerStandingOnCondition : Condition<PlayerContext> {
	public FootstepSurfacesRegistry Value { get; private set; }

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		if (!context.Player) return false;
		if (!StartOfRound.Instance) return false;

		FootstepSurface[] footstepSurfaces = StartOfRound.Instance.footstepSurfaces;
		if (footstepSurfaces == null) return false;

		int currentFootstepSurfaceIndex = context.Player.currentFootstepSurfaceIndex;
		if (currentFootstepSurfaceIndex < 0 || currentFootstepSurfaceIndex > footstepSurfaces.Length) return false;

		return Value.ContainsValue(footstepSurfaces[currentFootstepSurfaceIndex]);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		if (!GameNetworkManager.Instance.localPlayerController) return false;

		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}
}