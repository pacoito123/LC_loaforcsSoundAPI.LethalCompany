using GameNetcodeStuff;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:location")]
public class PlayerLocationCondition : Condition<PlayerContext> {
	public enum LocationType {
		INSIDE,
		ON_SHIP,
		OUTSIDE
	}

	public LocationType Value { get; internal set; }

	protected override bool EvaluateWithContext(PlayerContext context) {
		return context.Player != null && !context.Player.isPlayerDead
			&& (context.Player.isInsideFactory ? Value == LocationType.INSIDE
			: context.Player.isInHangarShipRoom ? Value == LocationType.ON_SHIP
			: Value == LocationType.OUTSIDE);
	}

	protected override bool EvaluateFallback(IContext context) {
		return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
			&& EvaluateWithContext(new PlayerContext(GameNetworkManager.Instance.localPlayerController));
	}
	// todo: validate
}