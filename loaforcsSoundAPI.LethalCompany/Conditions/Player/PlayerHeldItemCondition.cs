using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Player;

[SoundAPICondition("LethalCompany:player:held_item")]
public class PlayerHeldItemCondition : MultipleCondition<Item, PlayerContext> {
    private static Dictionary<string, Item>? _cachedItems;

    /// <inheritdoc/>
    protected override void OnRegistered() {
        if(string.IsNullOrEmpty(Value)) return;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void PopulateValues() {
        base.PopulateValues();
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

        if(StartOfRound.Instance == null || StartOfRound.Instance.allItemsList == null || StartOfRound.Instance.allItemsList.itemsList == null) return false;

        int totalItems = StartOfRound.Instance.allItemsList.itemsList.Count;
        _cachedItems ??= new(totalItems);

        match = match.ToLowerInvariant();
        if(!_cachedItems.TryGetValue(match, out value)) {
            for(int i = 0; i < totalItems; i++) {
                value = StartOfRound.Instance.allItemsList.itemsList[i];
                if(value != null && string.Equals(value.itemName, match, StringComparison.InvariantCultureIgnoreCase)) break;
                value = null!;
            }
        }

        return value != null;
    }

    /// <inheritdoc/>
    public override bool EvaluateFallback(IContext context) {
        return _currentContext.HasValue ? EvaluateWithContext(_currentContext.Value)
            : GameNetworkManager.Instance != null && GameNetworkManager.Instance.localPlayerController != null
                && EvaluateWithContext(new(context.Source, GameNetworkManager.Instance.localPlayerController));
    }
}