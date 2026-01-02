using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(EnemyAI))]
internal static class EnemyAIPatch {
    [HarmonyPatch(nameof(EnemyAI.Start)), HarmonyPrefix]
    private static void UpdateEnemyContexts(EnemyAI __instance) {
        EnemyContext context = new(__instance);
        foreach (AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = context;
        }
    }

    [HarmonyPatch(nameof(EnemyAI.Start)), HarmonyPostfix]
    private static void ReportEnemyStates(EnemyAI __instance) {
        if (SoundReportHandler.CurrentReport == null) return;
        if (__instance.enemyType == null) return;
        if (__instance.enemyBehaviourStates == null) return;
        if (__instance.enemyBehaviourStates.Length == 0) return;

        LethalCompanySoundReport.foundEnemyBehaviourStates.TryAdd(__instance.enemyType, __instance.enemyBehaviourStates);
    }
}