using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Data;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:behaviour_state")]
public class EnemyBehaviourStateCondition : Condition<EnemyContext> {
    public RangeOperator<int> Value { get; private set; } = new(0, int.MaxValue);

    [CanBeNull]
    public List<EnemyContentReference> EnemyName { get; private set; }

    /// <inheritdoc/>
	public override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;

        bool result = Value.EvaluateRange(context.Enemy.currentBehaviourStateIndex);
        if (result && EnemyName != null) {
            result = EnemyName.Find(it => it.Value == context.Enemy.enemyType) != null;
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
}