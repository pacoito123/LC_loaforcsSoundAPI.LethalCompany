using System;
using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class EnemyContentReference : ContentReference<EnemyType> {
    static EnemyType[] _allEnemyTypes;
    static Dictionary<string, EnemyType> _cachedEnemies;

    public EnemyContentReference(string input) : base(input) => StartOfRoundPatch.StartOfRoundAwake += Resolve;

    /// <inheritdoc/>
    protected override void OnResolved() => StartOfRoundPatch.StartOfRoundAwake -= Resolve;

    /// <inheritdoc/>
	protected override bool TryResolve(string input, out EnemyType value) {
        value = null;

        _allEnemyTypes ??= Resources.FindObjectsOfTypeAll<EnemyType>();
        _cachedEnemies ??= new(_allEnemyTypes.Length);

        if (string.IsNullOrEmpty(input)) return false;

        input = input.ToLowerInvariant();
        if (!_cachedEnemies.TryGetValue(input, out value)) {
            for (int i = 0; i < _allEnemyTypes.Length; i++) {
                value = _allEnemyTypes[i];
                if (value != null && string.Equals(value.enemyName, input, StringComparison.InvariantCultureIgnoreCase)
                    && _cachedEnemies.TryAdd(input, value)) break;
                value = null;
            }
        }

        return value != null;
    }
}