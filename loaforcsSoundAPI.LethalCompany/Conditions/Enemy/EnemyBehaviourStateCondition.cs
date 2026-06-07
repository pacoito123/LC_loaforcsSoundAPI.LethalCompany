using System;
using System.Collections.Generic;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:behaviour_state")]
public class EnemyBehaviourStateCondition : MultipleCondition<EnemyBehaviourState, EnemyContext> {
    protected override string ValidateWarnMessage => $"Value field for a MoonNameCondition in SoundPack '{Pack.Name}' is empty or missing!";

    public string EnemyName { get; private set; }

    /// <inheritdoc/>
    public override void OnRegistered() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
        StartOfRoundPatch.StartOfRoundAwake += PopulateValues;
    }

    /// <inheritdoc/>
    protected override void OnValuesPopulated() {
        StartOfRoundPatch.StartOfRoundAwake -= PopulateValues;
    }

    /// <inheritdoc/>
    protected override bool TryCacheValue(out EnemyBehaviourState value, string match) {
        value = null;

        if (string.IsNullOrEmpty(match)) return false;
        if (string.IsNullOrEmpty(EnemyName)) return false;
        if (!EnemyContext.TryFindEnemy(EnemyName, out EnemyAI enemy)) {
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Enemy name field '{Value}' for one \"LethalCompany:enemy:behaviour_state\" condition in SoundPack '{Pack.Name}' returned no successful matches!");
        }

        if (!enemy) return false;
        if (enemy.enemyBehaviourStates == null) return false;
        value = Array.Find(enemy.enemyBehaviourStates, enemyState => enemyState != null
            && string.Equals(enemyState.name, match, StringComparison.InvariantCultureIgnoreCase));

        if (value == null) {
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] EnemyBehaviourState field '{Value}' for one \"LethalCompany:enemy:behaviour_state\" condition in SoundPack '{Pack.Name}' returned no successful matches!");
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    protected override bool TryObtainValueWithContext(out EnemyBehaviourState value, EnemyContext context) {
        value = null;
        if (!context.Enemy) return false;
        value = context.Enemy.currentBehaviourState;
        return value != null;
    }

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        return !string.IsNullOrEmpty(EnemyName) ? base.Validate() : [new(IValidatable.ResultType.FAIL,
            $"Enemy name field for a \"LethalCompany:enemy:behaviour_state\" condition in SoundPack '{Pack.Name}' is empty or missing!")];
    }
}