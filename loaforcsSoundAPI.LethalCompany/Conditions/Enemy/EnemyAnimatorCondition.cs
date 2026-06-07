using JetBrains.Annotations;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Patches;
using loaforcsSoundAPI.SoundPacks.Conditions;
using loaforcsSoundAPI.SoundPacks.Data.Conditions;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Conditions.Enemy;

[SoundAPICondition("LethalCompany:enemy:animator")]
public sealed class EnemyAnimatorCondition : AnimatorCondition<EnemyContext> {
    protected override string ValidateWarnMessage => $"A parameter for an EnemyAnimatorCondition in SoundPack '{Pack.Name}' is empty or missing!";

    [CanBeNull]
    public string EnemyName { get; private set; } = null;
    EnemyType _enemyType;

    /// <inheritdoc/>
    public override void OnRegistered() {
        if (string.IsNullOrEmpty(EnemyName)) return;
        StartOfRoundPatch.StartOfRoundAwake -= CollectEnemy;
        StartOfRoundPatch.StartOfRoundAwake += CollectEnemy;
    }

    void CollectEnemy() {
        StartOfRoundPatch.StartOfRoundAwake -= CollectEnemy;

        if (!EnemyContext.TryFindEnemy(EnemyName, out EnemyAI enemy)) {
            Pack.Logger.LogWarning($"[Debug-SoundReplacementLoader] Enemy name field '{EnemyName}' for one \"LethalCompany:enemy:animator\" condition in SoundPack '{Pack.Name}' returned no successful matches!");
            _parameterType = AnimatorParamType.None;
            return;
        }

        _enemyType = enemy.enemyType;
    }

    /// <inheritdoc/>
    public override bool EvaluateWithContext(EnemyContext context) {
        bool result = true;

        if (context.Enemy) {
            result = base.EvaluateWithContext(context);
            if (result && _enemyType) {
                result = context.Enemy.enemyType == _enemyType;
            }
        }

        return result;
    }

    /// <inheritdoc/>
    protected override bool TryGetAnimator(out Animator animator, EnemyContext context) {
        animator = context.Enemy ? context.Enemy.creatureAnimator : null;
        return !animator;
    }
}