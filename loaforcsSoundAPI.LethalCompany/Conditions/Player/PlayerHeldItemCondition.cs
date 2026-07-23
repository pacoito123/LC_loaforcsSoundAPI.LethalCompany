using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:held_item")]
public class PlayerHeldItemCondition : Condition<PlayerContext> {
    public List<ItemContentReference> Value { get; private set; }

    /// <inheritdoc/>
	public override bool EvaluateWithContext(PlayerContext context) {
        if (!context.Player) return false;
        if (!context.Player.currentlyHeldObjectServer) return false;
        if (context.Player.isPlayerDead) return false;

        return Value.Find(reference => reference.Value == context.Player.currentlyHeldObjectServer.itemProperties) != null;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!GameNetworkManager.Instance) return false;
        if (!GameNetworkManager.Instance.localPlayerController) return false;

        return EvaluateWithContext(new PlayerContext(context?.Source, GameNetworkManager.Instance.localPlayerController));
    }
}