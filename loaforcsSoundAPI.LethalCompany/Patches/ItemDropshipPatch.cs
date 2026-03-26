using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(ItemDropship))]
internal static class ItemDropshipPatch {
    [HarmonyPatch(nameof(ItemDropship.Start)), HarmonyPostfix]
    private static void UpdateDropshipContexts(ItemDropship __instance) {
        DropshipContext.FallbackDropship = __instance;

        foreach(AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = new DropshipContext(source, __instance); ;
        }
    }
}