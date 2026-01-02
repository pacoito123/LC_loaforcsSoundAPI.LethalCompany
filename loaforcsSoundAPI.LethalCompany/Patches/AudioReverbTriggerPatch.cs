using HarmonyLib;
using loaforcsSoundAPI.LethalCompany.Reporting;
using loaforcsSoundAPI.Reporting;

namespace loaforcsSoundAPI.LethalCompany.Patches;

[HarmonyPatch(typeof(AudioReverbTrigger))]
static class AudioReverbTriggerPatch {
	[HarmonyPatch(nameof(AudioReverbTrigger.ChangeAudioReverbForPlayer)), HarmonyPostfix, HarmonyWrapSafe]
	static void LogFoundReverbPreset(AudioReverbTrigger __instance) {
		if (SoundReportHandler.CurrentReport == null) return;
		if (__instance.reverbPreset == null) return;

		LethalCompanySoundReport.foundReverbPresets.Add(__instance.reverbPreset);
	}
}