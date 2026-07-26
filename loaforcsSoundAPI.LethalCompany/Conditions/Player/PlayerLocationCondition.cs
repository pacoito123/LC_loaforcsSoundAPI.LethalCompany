using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:location")]
public class PlayerLocationCondition : Condition<PlayerContext> {
	public List<EnumReference<LocationType>> Value { get; private set; }

	/// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
		if (!context.Player) return false;
		if (context.Player.isPlayerDead) return false;

		LocationType currentLocation = context.Player.isInsideFactory ? LocationType.INSIDE
			: context.Player.isInHangarShipRoom ? LocationType.IN_SHIP
			: context.Player.isInElevator ? LocationType.ON_SHIP
			: LocationType.OUTSIDE;

		return Value.FindIndex(reference => reference.Value == currentLocation) != -1;
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
				new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"Value field for one \"LethalCompany:player:location\" condition in SoundPack '{Pack.Name}' is empty or missing!")
			];
		}

		return [];
	}
}

public enum LocationType : byte {
	INSIDE,
	IN_SHIP,
	ON_SHIP,
	OUTSIDE
}