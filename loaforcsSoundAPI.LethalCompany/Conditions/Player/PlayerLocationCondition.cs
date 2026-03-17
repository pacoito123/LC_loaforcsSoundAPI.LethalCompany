using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:location")]
public class PlayerLocationCondition : MultipleCondition<LocationType, PlayerContext> {
	/// <inheritdoc/>
	protected override bool TryGetValue(out LocationType location, string match) {
		return Enum.TryParse(match, ignoreCase: true, out location);
	}

	/// <inheritdoc/>
	protected override bool CheckValueWithContext(LocationType locationType, PlayerContext? context) {
		return context?.Player != null && !context.Player.isPlayerDead
			&& (context.Player.isInsideFactory ? locationType is LocationType.INSIDE
			: context.Player.isInHangarShipRoom ? locationType is LocationType.IN_SHIP
			: context.Player.isInElevator ? locationType is LocationType.ON_SHIP
			: locationType is LocationType.OUTSIDE);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new(GameNetworkManager.Instance.localPlayerController));
	}
}

public enum LocationType {
	INSIDE,
	IN_SHIP,
	ON_SHIP,
	OUTSIDE
}