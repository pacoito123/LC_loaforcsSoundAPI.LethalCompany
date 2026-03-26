using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:behaviour_state")]
public class EnemyBehaviourStateCondition : MultipleCondition<EnemyBehaviourState, EnemyContext> {
    private static EnemyType[]? _allEnemyTypes;
    private static Dictionary<string, EnemyAI>? _cachedEnemies;

    [CanBeNull]
    public string EnemyName { get; private set; } = null!;

    /// <inheritdoc/>
    protected override void OnRegistered() {
        if(string.IsNullOrEmpty(Value)) return;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void PopulateValues() {
        _allEnemyTypes ??= Resources.FindObjectsOfTypeAll<EnemyType>();
        _cachedEnemies ??= new(_allEnemyTypes.Length);
        base.PopulateValues();
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
    }

    /// <inheritdoc/>
    protected override bool CheckValueWithContext(EnemyBehaviourState value, EnemyContext context) {
        return context.Enemy != null && context.Enemy.currentBehaviourState == value;
    }

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        return !string.IsNullOrEmpty(EnemyName) ? base.Validate() : [new(IValidatable.ResultType.FAIL,
            $"Enemy name field for a \"LethalCompany:enemy:behaviour_state\" condition in SoundPack '{Pack.Name}' is empty or missing!")];
    }

    /// <inheritdoc/>
    protected override bool TryGetValue(out EnemyBehaviourState enemyState, string match) {
        enemyState = null!;

        if(_cachedEnemies == null || _allEnemyTypes == null || string.IsNullOrEmpty(EnemyName)) return false;

        string enemyName = EnemyName.ToLowerInvariant();
        if(!_cachedEnemies.TryGetValue(enemyName, out EnemyAI enemy)) {
            foreach(EnemyType enemyType in _allEnemyTypes) {
                if(enemyType != null && string.Equals(enemyType.enemyName, enemyName, StringComparison.InvariantCultureIgnoreCase)
                    && enemyType.enemyPrefab != null && enemyType.enemyPrefab.TryGetComponent(out enemy)
                    && _cachedEnemies.TryAdd(enemyName, enemy)) break;
                enemy = null!;
            }
        }

        if(enemy != null && enemy.enemyBehaviourStates != null) {
            enemyState = Array.Find(enemy.enemyBehaviourStates, enemyState =>
                string.Equals(enemyState?.name, match, StringComparison.InvariantCultureIgnoreCase));
        }

        return enemyState != null;
    }
}