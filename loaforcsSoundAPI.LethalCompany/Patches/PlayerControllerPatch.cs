using GameNetcodeStuff;
using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
static class PlayerControllerPatch {
	[HarmonyPatch(nameof(PlayerControllerB.Start))]
	static void UpdatePlayerContexts(PlayerControllerB __instance) {
		AudioSourceAdditionalData.GetOrCreate(__instance.movementAudio).CurrentContext = new PlayerContext(__instance.movementAudio, __instance);
		AudioSourceAdditionalData.GetOrCreate(__instance.statusEffectAudio).CurrentContext = new PlayerContext(__instance.statusEffectAudio, __instance);
		AudioSourceAdditionalData.GetOrCreate(__instance.waterBubblesAudio).CurrentContext = new PlayerContext(__instance.waterBubblesAudio, __instance);

		foreach(AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
			AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = new PlayerContext(source, __instance); ;
		}
	}
}