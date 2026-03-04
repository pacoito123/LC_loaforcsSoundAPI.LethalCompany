using System;
using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Conditions;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(StartOfRound))]
static class StartOfRoundPatch {
	internal static event Action? StartOfRoundAwake;

	[HarmonyPrefix, HarmonyPatch(nameof(StartOfRound.EndOfGame))]
	static void ResetApparatusState() {
		ApparatusStateCondition.CurrentApparatusPulled = false;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(StartOfRound.Awake))]
	static void StartOfRoundAwakePost(StartOfRound __instance) {
		if(SoundReportHandler.CurrentReport != null) {
			for(int i = 0; i < __instance.footstepSurfaces.Length; i++) {
				FootstepSurface? surface = __instance.footstepSurfaces[i];
				if(surface != null) LethalCompanySoundReport.foundFootstepSurfaces.Add(surface);
			}
		}
		StartOfRoundAwake?.Invoke();
	}

	// todo
	/*
	[HarmonyPostfix, HarmonyPatch(nameof(StartOfRound.OnPlayerConnectedClientRpc)), HarmonyWrapSafe]
	static void PlayShipSpeakerOnClientJoin(StartOfRound __instance) {
		if(!SoundFixesConfig.PlayShipSpeakerOnClientJoin.Value) return;
		if(__instance.IsServer || __instance.IsHost) return;
		if (__instance.gameStats.daysSpent == 0) {
			__instance.speakerAudioSource.PlayOneShot(__instance.shipIntroSpeechSFX);
		}
	}
	*/
}