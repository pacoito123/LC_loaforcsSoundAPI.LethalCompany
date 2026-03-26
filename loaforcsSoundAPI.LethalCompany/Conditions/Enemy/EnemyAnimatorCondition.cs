using JetBrains.Annotations;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:animator")]
public sealed class EnemyAnimatorCondition : AnimatorCondition<EnemyContext> {
    [CanBeNull]
    public string EnemyName { get; private set; } = null!;
    private EnemyType? _enemyType;

    /// <inheritdoc/>
    protected override void OnRegistered() {
        if(string.IsNullOrEmpty(EnemyName)) return;

        StartOfRoundPatch.StartOfRoundAwake += CollectEnemy;
    }

    private void CollectEnemy() {
        StartOfRoundPatch.StartOfRoundAwake -= CollectEnemy;

        if(!EnemyContext.TryFindEnemy(EnemyName, out EnemyAI enemy)) {
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Enemy name field '{EnemyName}' for one \"LethalCompany:enemy:animator\" condition in SoundPack '{Pack.Name}' returned no successful matches!");
            _parameterType = AnimatorParamType.None;
            return;
        }

        _enemyType = enemy.enemyType;
    }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(EnemyContext context) {
        return (_enemyType == null || string.IsNullOrEmpty(EnemyName) || (context.Enemy != null && context.Enemy.enemyType != _enemyType))
            && base.EvaluateWithContext(context);
    }

    /// <inheritdoc/>
    protected override bool TryGetAnimator(out Animator animator, EnemyContext context) {
        animator = (context.Enemy != null) ? context.Enemy.creatureAnimator : null!;
        return animator != null;
    }
}