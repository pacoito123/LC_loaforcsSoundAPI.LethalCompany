using GameNetcodeStuff;
using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(VehicleController))]
static class VehicleControllerPatch {
    [HarmonyPatch(nameof(VehicleController.Start))]
    static void UpdateVehicleContexts(VehicleController __instance) {
        VehicleContext context = new(__instance);
        foreach (AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
            AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = context;
        }
    }
}