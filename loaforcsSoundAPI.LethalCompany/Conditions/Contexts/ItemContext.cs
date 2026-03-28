using System;
using System.Collections.Generic;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Contexts;

public struct ItemContext(AudioSource? source, GrabbableObject? item) : IContext {
    public readonly AudioSource? Source => source;
    public readonly GrabbableObject? Item => item;
    public readonly Item? ItemProperties => (item != null) ? item.itemProperties : null;

    internal static Dictionary<string, Item>? _cachedItems;

    internal static bool TryFindItem(string itemName, out Item item) {
        item = null!;

        if(StartOfRound.Instance == null || StartOfRound.Instance.allItemsList == null || StartOfRound.Instance.allItemsList.itemsList == null) return false;

        int totalItems = StartOfRound.Instance.allItemsList.itemsList.Count;
        _cachedItems ??= new(totalItems);

        itemName = itemName.ToLowerInvariant();
        if(!_cachedItems.TryGetValue(itemName, out item)) {
            for(int i = 0; i < totalItems; i++) {
                item = StartOfRound.Instance.allItemsList.itemsList[i];
                if(item != null && string.Equals(item.itemName, itemName, StringComparison.InvariantCultureIgnoreCase)
                    && _cachedItems.TryAdd(itemName, item)) break;
                item = null!;
            }
        }

        return item != null;
    }
}