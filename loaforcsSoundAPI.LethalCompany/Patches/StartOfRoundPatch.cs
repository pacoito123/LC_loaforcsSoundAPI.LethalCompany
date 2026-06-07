using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;
using System;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(StartOfRound))]
static class StartOfRoundPatch {
	internal static event Action StartOfRoundAwake = delegate { };

	[HarmonyPatch(nameof(StartOfRound.Awake)), HarmonyPostfix]
	static void ReportFootstepSurfaces(FootstepSurface[] ___footstepSurfaces) {
		if (SoundReportHandler.CurrentReport != null && ___footstepSurfaces != null) {
			LethalCompanySoundReport.foundFootstepSurfaces.UnionWith(___footstepSurfaces);
		}
		StartOfRoundAwake();
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