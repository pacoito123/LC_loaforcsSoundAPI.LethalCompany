using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Data;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Data;

public class EnemiesRegistry : Registry<EnemyType, HashSet<EnemyType>> {
    static EnemyType[] _allEnemyTypes;
    static Dictionary<string, EnemyType> _cachedEnemies;

    public EnemiesRegistry() : base() => StartOfRoundPatch.StartOfRoundAwake += PopulateRegistry;

    /// <inheritdoc/>
    public override void OnRegistryPopulated() => StartOfRoundPatch.StartOfRoundAwake -= PopulateRegistry;

    /// <inheritdoc/>
    public override bool TryParse(out EnemyType value, JToken token) {
        value = null;

        _allEnemyTypes ??= Resources.FindObjectsOfTypeAll<EnemyType>();
        _cachedEnemies ??= new(_allEnemyTypes.Length);

        string match = token.ToString();
        if (string.IsNullOrEmpty(match)) return false;

        match = match.ToLowerInvariant();
        if (!_cachedEnemies.TryGetValue(match, out value)) {
            for (int i = 0; i < _allEnemyTypes.Length; i++) {
                value = _allEnemyTypes[i];
                if (value != null && string.Equals(value.enemyName, match, System.StringComparison.InvariantCultureIgnoreCase)
                    && _cachedEnemies.TryAdd(match, value)) break;
                value = null;
            }
        }

        return value != null;
    }
}