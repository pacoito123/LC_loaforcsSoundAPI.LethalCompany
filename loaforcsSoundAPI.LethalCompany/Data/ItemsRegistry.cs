using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class ItemsRegistry : Registry<Item, HashSet<Item>> {
    internal static Dictionary<string, Item> _cachedItems;

    public ItemsRegistry() : base() => StartOfRoundPatch.StartOfRoundAwake += PopulateRegistry;

	/// <inheritdoc/>
    public override void OnRegistryPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateRegistry;

	/// <inheritdoc/>
    public override bool TryParse(out Item value, JToken token) {
        value = null;

        if (!StartOfRound.Instance) return false;
        if (!StartOfRound.Instance.allItemsList) return false;
        if (StartOfRound.Instance.allItemsList.itemsList == null) return false;

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        int totalItems = StartOfRound.Instance.allItemsList.itemsList.Count;
        _cachedItems ??= new(totalItems);

        match = match.ToLowerInvariant();
        if (!_cachedItems.TryGetValue(match, out value)) {
            for (int i = 0; i < totalItems; i++) {
                value = StartOfRound.Instance.allItemsList.itemsList[i];
                if (value != null && string.Equals(value.itemName, match, System.StringComparison.InvariantCultureIgnoreCase)
                    && _cachedItems.TryAdd(match, value)) break;
                value = null;
            }
        }

        return value != null;
    }
}