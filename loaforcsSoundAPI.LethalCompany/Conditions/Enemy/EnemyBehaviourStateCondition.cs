using JetBrains.Annotations;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:behaviour_state")]
public class EnemyBehaviourStateCondition : RangeCondition<int, EnemyContext> {
    /// <inheritdoc/>
    protected override RangeOperator<int> DefaultRange => new(0, int.MaxValue);

    [CanBeNull]
    public EnemiesRegistry EnemyName { get; private set; }

    /// <inheritdoc/>
	public override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;

        bool result = EvaluateRangeOperator(context.Enemy.currentBehaviourStateIndex);
        if (result && EnemyName != null) {
            result = EnemyName.ContainsValue(context.Enemy.enemyType);
        }

        return result;
    }

    /// <inheritdoc/>
    protected override bool TryParseValue(string parameter, ref int value) {
        return string.IsNullOrEmpty(parameter) || int.TryParse(parameter, out value);
    }
}