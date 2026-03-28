using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:behaviour_state")]
public class EnemyBehaviourStateCondition : MultipleCondition<EnemyBehaviourState, EnemyContext> {
    [CanBeNull]
    public string EnemyName { get; private set; } = null!;

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
    protected override bool CheckValueWithContext(EnemyBehaviourState value, EnemyContext context) {
        return context.Enemy != null && context.Enemy.currentBehaviourState == value;
    }

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        return !string.IsNullOrEmpty(EnemyName) ? base.Validate() : [new(IValidatable.ResultType.FAIL,
            $"Enemy name field for a \"LethalCompany:enemy:behaviour_state\" condition in SoundPack '{Pack.Name}' is empty or missing!")];
    }

    /// <inheritdoc/>
    protected override bool TryGetValue(out EnemyBehaviourState value, string match) {
        value = null!;

        if(string.IsNullOrEmpty(EnemyName)) return false;
        if(EnemyContext.TryFindEnemy(EnemyName, out EnemyAI enemy) && enemy.enemyBehaviourStates != null) {
            value = Array.Find(enemy.enemyBehaviourStates, enemyState =>
                string.Equals(enemyState?.name, match, StringComparison.InvariantCultureIgnoreCase));
        }

        return value != null;
    }
}