using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(EnemyAI))]
static class EnemyAIPatch {
    [HarmonyPatch(nameof(EnemyAI.Start)), HarmonyPrefix]
    static void UpdateEnemyContexts(EnemyAI __instance) {
        foreach (AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = new EnemyContext(source, __instance);
        }
    }

    [HarmonyPatch(nameof(EnemyAI.Start)), HarmonyPostfix]
    static void ReportEnemyStates(EnemyType ___enemyType, EnemyBehaviourState[] ___enemyBehaviourStates) {
        if (SoundReportHandler.CurrentReport == null) return;
        if (!___enemyType) return;
        if (___enemyBehaviourStates == null) return;

        LethalCompanySoundReport.foundEnemyBehaviourStates.TryAdd(___enemyType, ___enemyBehaviourStates);
    }
}