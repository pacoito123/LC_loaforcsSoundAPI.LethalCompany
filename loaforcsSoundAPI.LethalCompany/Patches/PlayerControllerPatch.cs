using GameNetcodeStuff;
using HarmonyLib;
using loaforcsSoundAPI.Core;
using loaforcsSoundAPI.LethalCompany.Conditions.Contexts;
using UnityEngine;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
static class PlayerControllerPatch {
	[HarmonyPatch(nameof(PlayerControllerB.Start)), HarmonyPrefix]
	static void UpdatePlayerContexts(PlayerControllerB __instance, AudioSource ___movementAudio, AudioSource ___statusEffectAudio, AudioSource ___waterBubblesAudio) {
		if (___movementAudio) {
			AudioSourceAdditionalData.GetOrCreate(___movementAudio).CurrentContext = new PlayerContext(___movementAudio, __instance);
		}
		if (___statusEffectAudio) {
			AudioSourceAdditionalData.GetOrCreate(___statusEffectAudio).CurrentContext = new PlayerContext(___statusEffectAudio, __instance);
		}
		if (___waterBubblesAudio) {
			AudioSourceAdditionalData.GetOrCreate(___waterBubblesAudio).CurrentContext = new PlayerContext(___waterBubblesAudio, __instance);
		}

		foreach (AudioSource source in __instance.GetComponentsInChildren<AudioSource>(includeInactive: true)) {
			AudioSourceAdditionalData.GetOrCreate(source).CurrentContext = new PlayerContext(source, __instance); ;
		}
	}
}