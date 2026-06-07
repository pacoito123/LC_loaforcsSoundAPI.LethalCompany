using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(GrabbableObject))]
static class GrabbableObjectPatch {
    [HarmonyPatch(nameof(GrabbableObject.Start)), HarmonyPrefix]
    static void UpdateItemContexts(GrabbableObject __instance) {
        foreach (AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = new ItemContext(source, __instance);
        }
    }
}