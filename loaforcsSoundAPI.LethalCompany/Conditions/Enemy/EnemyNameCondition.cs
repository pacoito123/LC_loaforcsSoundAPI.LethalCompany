using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:name")]
public sealed class EnemyNameCondition : Condition<EnemyContext> {
    public EnemiesRegistry Value { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;

        return Value.ContainsValue(context.Enemy.enemyType);
    }
}