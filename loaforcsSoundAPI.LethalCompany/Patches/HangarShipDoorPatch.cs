using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(HangarShipDoor))]
internal static class HangarShipDoorPatch {
    [HarmonyPatch(nameof(HangarShipDoor.Start)), HarmonyPrefix]
    private static void UpdateEnemyContexts(HangarShipDoor __instance) {
        ShipDoorContext.FallbackShipDoor = __instance;

        foreach(AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = new ShipDoorContext(source, __instance);
        }
    }
}