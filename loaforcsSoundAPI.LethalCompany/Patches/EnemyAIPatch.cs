using GameNetcodeStuff;
using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(EnemyAI))]
static class EnemyAIPatch {
    [HarmonyPatch(nameof(EnemyAI.Start))]
    static void UpdateEnemyContexts(EnemyAI __instance) {
        EnemyContext context = new(__instance);
        foreach (AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = context;
        }
    }
}