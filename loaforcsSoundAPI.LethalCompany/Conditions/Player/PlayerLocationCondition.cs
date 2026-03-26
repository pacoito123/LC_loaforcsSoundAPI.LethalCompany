using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:location")]
public class PlayerLocationCondition : MultipleCondition<LocationType, PlayerContext> {
	/// <inheritdoc/>
	protected override bool TryGetValue(out LocationType value, string match) {
		return Enum.TryParse(match, ignoreCase: true, out value);
	}

	/// <inheritdoc/>
	protected override bool CheckValueWithContext(LocationType value, PlayerContext context) {
		return context.Player != null && !context.Player.isPlayerDead
			&& (context.Player.isInsideFactory ? value is LocationType.INSIDE
			: context.Player.isInHangarShipRoom ? value is LocationType.IN_SHIP
			: context.Player.isInElevator ? value is LocationType.ON_SHIP
			: value is LocationType.OUTSIDE);
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		return _currentContext.HasValue ? EvaluateWithContext(_currentContext.Value)
			: GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
				&& EvaluateWithContext(new(context.Source, GameNetworkManager.Instance.localPlayerController));
	}
}

public enum LocationType : byte {
	INSIDE,
	IN_SHIP,
	ON_SHIP,
	OUTSIDE
}