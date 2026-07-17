using JetBrains.Annotations;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Data;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:animator")]
public sealed class EnemyAnimatorCondition : AnimatorCondition<EnemyContext> {
    /// <inheritdoc/>
    protected override string ValidateWarnMessage => $"A parameter for an EnemyAnimatorCondition in SoundPack '{Pack.Name}' is empty or missing!";

    [CanBeNull]
    public EnemiesRegistry EnemyName { get; private set; }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(EnemyContext context) {
        if (!context.Enemy) return false;

        bool result = base.EvaluateWithContext(context);
        if (result && EnemyName != null) {
            result = EnemyName.ContainsValue(context.Enemy.enemyType);
        }

        return result;
    }

    /// <inheritdoc/>
    protected override bool TryGetAnimator(out Animator animator, EnemyContext context) {
        animator = null;
        if (!context.Enemy) return false;

        animator = context.Enemy.creatureAnimator;

        return animator;
    }
}