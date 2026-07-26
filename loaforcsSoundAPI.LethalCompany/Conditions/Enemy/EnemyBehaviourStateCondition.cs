using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
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
    public List<EnemyContentReference> EnemyName { get; private set; }

    /// <inheritdoc/>
	public override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;

        bool result = EvaluateRangeOperator(context.Enemy.currentBehaviourStateIndex);
        if (result && EnemyName != null) {
            result = EnemyName.Find(reference => reference.Value == context.Enemy.enemyType) != null;
        }

        return result;
    }

    /// <inheritdoc/>
    public override List<IValidatable.ValidationResult> Validate() {
        if (EnemyName != null && EnemyName.Count == 0) {
            return [
                new IValidatable.ValidationResult(IValidatable.ResultType.FAIL, $"EnemyName field for one \"LethalCompany:enemy:behaviour_state\" condition in SoundPack '{Pack.Name}' is empty!")
            ];
        }

        return base.Validate();
    }

    /// <inheritdoc/>
    protected override bool TryParseValue(string parameter, ref int value) {
        return string.IsNullOrEmpty(parameter) || int.TryParse(parameter, out value);
    }
}