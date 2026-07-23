using System.Collections.Generic;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:name")]
public sealed class EnemyNameCondition : Condition<EnemyContext> {
    public List<EnemyContentReference> Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;

        return Value.Find(reference => reference.Value == context.Enemy.enemyType) != null;
    }
}