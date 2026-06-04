using System;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:location")]
public class PlayerLocationCondition : MultipleCondition<LocationType, PlayerContext> {
	protected override string ValidateWarnMessage => $"Value field for a PlayerLocationCondition in SoundPack '{Pack.Name}' is empty or missing!";

	/// <inheritdoc/>
	protected override bool TryCacheValue(out LocationType value, string match) {
		return Enum.TryParse(match, ignoreCase: true, out value);
	}

	/// <inheritdoc/>
	protected override bool TryObtainValueWithContext(out LocationType value, PlayerContext context) {
		value = default;
		if (!context.Player) return false;
		if (context.Player.isPlayerDead) return false;

		value = context.Player.isInsideFactory ? LocationType.INSIDE
			: context.Player.isInHangarShipRoom ? LocationType.IN_SHIP
			: context.Player.isInElevator ? LocationType.ON_SHIP
			: LocationType.OUTSIDE;
		return true;
	}

	/// <inheritdoc/>
	public override bool EvaluateFallback(IContext context) {
		if (!GameNetworkManager.Instance) return false;
		return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
	}
}

public enum LocationType : byte {
	INSIDE,
	IN_SHIP,
	ON_SHIP,
	OUTSIDE
}