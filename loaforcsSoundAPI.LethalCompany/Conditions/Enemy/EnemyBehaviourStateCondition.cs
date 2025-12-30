using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using loaforcsSoundAPI.Core.Data;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:behaviour_state")]
public class EnemyBehaviourStateCondition : Condition<EnemyContext> {
    [CanBeNull]
    public string EnemyName { get; private set; } = null;

    [CanBeNull]
    public string StateName { get; private set; } = null;

    [CanBeNull]
    public string StateIndex { get; private set; } = null;

    protected override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;
        if (!context.Enemy.enemyType) return false;
        if (context.Enemy.currentBehaviourState == null) return false;

        bool? result = null;

        if (EnemyName != null) {
            result = string.Equals(EnemyName, context.EnemyType.enemyName, StringComparison.InvariantCultureIgnoreCase);
        }

        if (StateName != null) {
            result = string.Equals(StateName, context.Enemy.currentBehaviourState.name, StringComparison.InvariantCultureIgnoreCase);
        }

        if (StateIndex != null) {
            result = EvaluateRangeOperator(context.Enemy.currentBehaviourStateIndex, StateIndex);
        }

        return result == true;
    }

	public override List<IValidatable.ValidationResult> Validate() {
		if (!ValidateRangeOperator(StateIndex, out IValidatable.ValidationResult result))
			return [result];
		return [];
	}
}