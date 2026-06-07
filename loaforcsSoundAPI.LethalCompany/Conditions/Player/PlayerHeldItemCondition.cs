using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:held_item")]
public class PlayerHeldItemCondition : MultipleCondition<Item, PlayerContext> {
    protected override string ValidateWarnMessage => $"Value field for a PlayerHeldItemCondition in SoundPack '{Pack.Name}' is empty or missing!";

    /// <inheritdoc/>
    public override void OnRegistered() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;

    /// <inheritdoc/>
    protected override bool TryCacheValue(out Item value, string match) {
        value = null;

        if (string.IsNullOrEmpty(match)) return false;
        if (!ItemContext.TryFindItem(match, out value)) {
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Item name field '{Value}' for one \"LethalCompany:player:held_item\" condition in SoundPack '{Pack.Name}' returned no successful matches!");
        }

        return value != null;
    }

    /// <inheritdoc/>
    protected override bool TryObtainValueWithContext(out Item value, PlayerContext context) {
        value = null;
        if (!context.Player) return false;
        if (!context.Player.currentlyHeldObjectServer) return false;

        value = context.Player.currentlyHeldObjectServer.itemProperties;
        return value != null;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        if (!GameNetworkManager.Instance) return false;
        return EvaluateWithContext(new(context?.Source, GameNetworkManager.Instance.localPlayerController));
    }
}