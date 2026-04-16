using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:held_item")]
public class PlayerHeldItemCondition : MultipleCondition<Item, PlayerContext> {
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
    protected override bool CheckValueWithContext(Item value, PlayerContext context) {
        return context.Player != null && context.Player.currentlyHeldObjectServer != null && context.Player.currentlyHeldObjectServer.itemProperties == value;
    }

    /// <inheritdoc/>
    protected override bool TryGetValue(out Item value, string match) {
        value = null!;

        if(string.IsNullOrEmpty(match) || !ItemContext.TryFindItem(match, out value))
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Item name field '{Value}' for one \"LethalCompany:player:held_item\" condition in SoundPack '{Pack.Name}' returned no successful matches!");

        return value != null;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        return GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
            && EvaluateWithContext(new(context.Source, GameNetworkManager.Instance.localPlayerController));
    }
}