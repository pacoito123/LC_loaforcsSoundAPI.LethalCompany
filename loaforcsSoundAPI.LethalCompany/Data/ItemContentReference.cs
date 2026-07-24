using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class ItemContentReference : ContentReference<Item> {
    static Dictionary<string, Item> _cachedItems;

    public ItemContentReference(string input) : base(input) => StartOfRoundPatch.StartOfRoundAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved(bool success) => StartOfRoundPatch.StartOfRoundAwake -= Resolve;

    /// <inheritdoc/>
	protected override bool TryResolve(string input, out Item value) {
        value = null;

        int totalItems = StartOfRound.Instance.allItemsList.itemsList.Count;
        _cachedItems ??= new(totalItems);

        if (string.IsNullOrEmpty(input)) return false;

        input = input.ToLowerInvariant();
        if (!_cachedItems.TryGetValue(input, out value)) {
            for (int i = 0; i < totalItems; i++) {
                value = StartOfRound.Instance.allItemsList.itemsList[i];
                if (value != null && string.Equals(value.itemName, input, StringComparison.InvariantCultureIgnoreCase)
                    && _cachedItems.TryAdd(input, value)) break;
                value = null;
            }
        }

        return value != null;
    }
}